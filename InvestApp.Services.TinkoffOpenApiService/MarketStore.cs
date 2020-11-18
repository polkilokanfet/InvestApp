using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Domain.Models.FinMod;
using InvestApp.Domain.Services;
using InvestApp.Domain.Services.DataBaseAccess;
using InvestApp.Services.TinkoffOpenApiService.Extensions;
using InvestApp.Services.TinkoffOpenApiService.Models;
using InvestApp.Services.TinkoffOpenApiService.Network;

namespace InvestApp.Services.TinkoffOpenApiService
{
    public class MarketStore : IMarketStore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFinancialModelingPrepService _financialModelingPrepService;
        private readonly IContext _context;

        public MarketStore(string token, bool isSandbox, IUnitOfWork unitOfWork, IMessageService messageService, IFinancialModelingPrepService financialModelingPrepService)
        {
            _unitOfWork = unitOfWork;
            _financialModelingPrepService = financialModelingPrepService;

            if (isSandbox)
            {
                SandboxConnection connection = ConnectionFactory.GetSandboxConnection(token);
                _context = connection.Context;
            }
            else
            {
                Connection connection = ConnectionFactory.GetConnection(token);
                _context = connection.Context;
            }

            _context.WebSocketException += (sender, exception) =>
            {
                messageService.ShowMessage(exception.Message);
            };
        }

        public async Task<IEnumerable<Transaction>> RefreshTransactionsAsync()
        {
            List<Transaction> result = new List<Transaction>();

            //транзакции из базы данных
            List<Transaction> transactions = _unitOfWork.Repository<Transaction>().GetAllAsNoTracking();
            List<string> tcsIds = transactions.Select(transaction => transaction.IdTcs).Where(id => id != null).Distinct().ToList();

            //загрузка транзакций из Api
            IEnumerable<Operation> operationsApi = await _context.OperationsAsync(new DateTime(2015, 9, 1), DateTime.Now, null);
            //var op = operationsApi.Where(x => x.OperationType == ExtendedOperationType.Repayment).ToList();
            List<Operation> operationsNew = operationsApi.Where(operation => !tcsIds.Contains(operation.Id)).ToList();

            if (operationsNew.Any())
            {
                List<Instrument> instruments = _unitOfWork.Repository<Instrument>().GetAll();
                List<string> figiList = instruments.Select(instrument => instrument.Figi).Where(figi => figi != null).Distinct().ToList();

                //обновление инструментов
                var ss = operationsNew.Select(operation => operation.Figi).Distinct().ToList();
                if (operationsNew.Select(operation => operation.Figi).Any(figi => !figiList.Contains(figi)))
                {
                    await RefreshMarketInstrumentsAsync();
                    instruments = _unitOfWork.Repository<Instrument>().GetAll();
                }

                foreach (Operation operation in operationsNew)
                {
                    Instrument instrument = instruments.SingleOrDefault(instr => instr.Figi == operation.Figi);
                    Transaction transaction = operation.ToTransaction(instrument);
                    _unitOfWork.Repository<Transaction>().Add(transaction);
                    result.Add(transaction);
                }

                _unitOfWork.SaveChanges();
            }

            return result;
        }

        public IEnumerable<IAsset> GetAssets()
        {
            List<Transaction> transactions = _unitOfWork.Repository<Transaction>().Find(transaction => transaction.Instrument != null);
            return transactions
                .GroupBy(transaction => transaction.Instrument)
                .Where(x => x.Any(transaction => transaction.Status != OperationStatus.Decline))
                .Select(x => new Asset(x, this));
        }

        public async Task<List<Instrument>> GetMarketInstruments()
        {
            var stocks = (await _context.MarketStocksAsync()).Instruments.Select(marketInstrument => marketInstrument.ToInstrument());
            var bonds = (await _context.MarketBondsAsync()).Instruments.Select(marketInstrument => marketInstrument.ToInstrument());
            var etfs = (await _context.MarketEtfsAsync()).Instruments.Select(marketInstrument => marketInstrument.ToInstrument());
            var currencies = (await _context.MarketCurrenciesAsync()).Instruments.Select(marketInstrument => marketInstrument.ToInstrument());

            return stocks.Union(bonds).Union(etfs).Union(currencies).ToList();
        }

        public async Task<List<Instrument>> RefreshMarketInstrumentsAsync(bool includeRefreshPrices = true)
        {
            //Инструменты из API
            IEnumerable<Instrument> instrumentsApi = await GetMarketInstruments();

            //Инструменты из бызы
            List<Instrument> instrumentsDb = _unitOfWork.Repository<Instrument>().GetAllAsNoTracking();
            List<string> figiList = instrumentsDb.Select(instrument => instrument.Figi).Where(figi => figi != null).Distinct().ToList();

            List<Instrument> result = new List<Instrument>();

            //добавление новых инструментов в базу данных
            foreach (Instrument instrument in instrumentsApi.Where(instrument => !figiList.Contains(instrument.Figi)))
            {
                //instrument.LastPrice = await GetPrice(instrument.Figi);
                _unitOfWork.Repository<Instrument>().Add(instrument);
                result.Add(instrument);
            }

            //сохранение новых инструментов в базе данных
            if (result.Any())
            {
                _unitOfWork.SaveChanges();
            }

            return result;
        }

        public async Task<decimal> RefreshInstrumentLastPriceAsync(string figi)
        {
            Instrument instrument = _unitOfWork.Repository<Instrument>().Find(instrument1 => instrument1.Figi == figi).Single();
            instrument.LastPrice = await GetPrice(figi);
            _unitOfWork.SaveChanges();
            return instrument.LastPrice;
        }

        public async Task<decimal> GetPrice(string figi, DateTime moment = default)
        {
            //по стакану
            if (moment == default)
                return await GetPriceByOrderbook(figi);

            //по истории свечей
            CandleList candleList = await _context.MarketCandlesAsync(figi, moment.AddDays(-7), moment, CandleInterval.Week);
            if (candleList.Candles.Any())
                return candleList.Candles.OrderBy(candle => candle.Time).Last().Close;
            
            throw new Exception($"Не удалось найти цену на {moment.ToShortDateString()} с FIGI {figi}");
        }

        public async Task<decimal> GetRubExchangeRate(Currency currency, DateTime moment = default)
        {
            if (currency == Currency.Rub)
                return 1;

            string figi;
            switch (currency)
            {
                case Currency.Usd:
                    figi = "BBG0013HGFT4";
                    break;
                case Currency.Eur:
                    figi = "BBG0013HJJ31";
                    break;
                case Currency.Gbp:
                    throw new NotImplementedException();
                case Currency.Hkd:
                    throw new NotImplementedException();
                case Currency.Chf:
                    throw new NotImplementedException();
                case Currency.Jpy:
                    throw new NotImplementedException();
                case Currency.Cny:
                    throw new NotImplementedException();
                case Currency.Try:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(currency), currency, null);
            }

            return await GetPrice(figi, moment);
        }

        /// <summary>
        /// Цена из стакана
        /// </summary>
        /// <param name="figi"></param>
        /// <returns></returns>
        private async Task<decimal> GetPriceByOrderbook(string figi)
        {
            //try
            //{
            var orderBook = await _context.MarketOrderbookAsync(figi, 1);
            return orderBook.LastPrice;
            //}
            //catch (OpenApiException e)
            //{
            //    Console.WriteLine(e);
            //}

            //return 0;
        }

        public async Task SellPositionAsync(string figi, int lots)
        {
            MarketOrder marketOrder = new MarketOrder(figi, lots, OperationType.Sell);
            await _context.PlaceMarketOrderAsync(marketOrder);
        }

        public async Task<CompanyProfile> GetCompanyProfileAsync(string symbol)
        {
            //по возможности возвращаем сохраненный профиль
            CompanyProfile companyProfile = _unitOfWork
                .Repository<CompanyProfile>()
                .Find(profile => profile.Symbol == symbol)
                .FirstOrDefault();
            if (companyProfile != null) return companyProfile;

            //загружаем профиль из сервиса
            CompanyProfileFinMod companyProfileFinMod = await _financialModelingPrepService.GetCompanyProfileAsync(symbol);
            companyProfile = new CompanyProfile
            {
                Cik = companyProfileFinMod.Cik, 
                Cusip = companyProfileFinMod.Cusip, 
                Currency = companyProfileFinMod.Currency, 
                CompanyName = companyProfileFinMod.CompanyName, 
                Description = companyProfileFinMod.Description, 
                Country = _unitOfWork.Repository<Country>().Find(country => country.Name == companyProfileFinMod.Country).SingleOrDefault() ?? new Country {Name = companyProfileFinMod.Country}, 
                Exchange = _unitOfWork.Repository<Exchange>().Find(exchange => exchange.ShortName == companyProfileFinMod.ExchangeShortName).SingleOrDefault() ?? new Exchange { ShortName = companyProfileFinMod.ExchangeShortName, FullName = companyProfileFinMod.Exchange }, 
                Industry = _unitOfWork.Repository<Industry>().Find(industry => industry.Name == companyProfileFinMod.Industry).SingleOrDefault() ?? new Industry { Name = companyProfileFinMod.Industry },
                Sector = _unitOfWork.Repository<Sector>().Find(sector => sector.Name == companyProfileFinMod.Sector).SingleOrDefault() ?? new Sector { Name = companyProfileFinMod.Sector },
                Image = companyProfileFinMod.Image, 
                IpoDate = companyProfileFinMod.IpoDate, 
                Isin = companyProfileFinMod.Isin, 
                Symbol = companyProfileFinMod.Symbol, 
                Website = companyProfileFinMod.Website
            };
            companyProfile.FinancialRatios = await GetFinancialRatiosAsync(companyProfile);
            _unitOfWork.Repository<CompanyProfile>().Add(companyProfile);
            _unitOfWork.SaveChanges();

            return companyProfile;
        }

        public async Task<List<FinancialRatio>> GetFinancialRatiosAsync(CompanyProfile company)
        {
            var result = await _financialModelingPrepService.GetFinancialRatiosAsync(company.Symbol);
            return result;
        }
    }
}
