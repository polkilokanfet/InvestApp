using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestApp.Domain.Exceptions;
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
            var orderBook = await _context.MarketOrderbookAsync(figi, 1);
            return orderBook.LastPrice;
        }

        public async Task<CompanyProfile> GetCompanyProfileAsync(string symbol)
        {
            //по возможности возвращаем сохраненный профиль
            CompanyProfile companyProfile = _unitOfWork.Repository<CompanyProfile>().Find(profile => profile.Symbol == symbol).SingleOrDefault();
            if (companyProfile != null) 
                return companyProfile;

            //если символов нет в сервисе
            if (_unitOfWork.Repository<FinancialModelingServiceInvalidSymbols>().Find(x => x.InvalidSymbols == symbol).Any())
                throw new NotImplementedException();

            //загружаем профиль из сервиса
            try
            {
                CompanyProfileFinMod companyProfileFinMod = await _financialModelingPrepService.GetCompanyProfileAsync(symbol);
                companyProfile = companyProfileFinMod.ToCompanyProfile(_unitOfWork);
                _unitOfWork.Repository<CompanyProfile>().Add(companyProfile);

                //финансовые показатели
                try
                {
                    companyProfile.FinancialRatios = await GetFinancialRatiosAsync(companyProfile);
                }
                catch
                {
                    // ignored
                }

                //рейтинг компании
                try
                {
                    CompanyRaiting companyRaiting = await GetCompanyRaitingAsync(companyProfile);
                    _unitOfWork.Repository<CompanyRaiting>().Add(companyRaiting);
                    companyProfile.CompanyRaitings = new List<CompanyRaiting> { companyRaiting };
                }
                catch
                {
                    // ignored
                }

                _unitOfWork.SaveChanges();
                return companyProfile;
            }
            catch (InvalidSymbolException)
            {
                //добавляем тикер в список отсутствующих в сервисе тикеров
                _unitOfWork.Repository<FinancialModelingServiceInvalidSymbols>().Add(new FinancialModelingServiceInvalidSymbols { InvalidSymbols = symbol });
                _unitOfWork.SaveChanges();
                throw;
            }
        }

        public async Task<List<FinancialRatio>> GetFinancialRatiosAsync(CompanyProfile company)
        {
            return await _financialModelingPrepService.GetFinancialRatiosAsync(company.Symbol);
        }

        #region GetCompanyRaiting

        public async Task<CompanyRaiting> GetCompanyRaitingAsync(CompanyProfile company)
        {
            CompanyRaitingFinMod raitingFinMod = await _financialModelingPrepService.GetCompanyRaitingAsync(company.Symbol);
            CompanyRaiting companyRaiting = new CompanyRaiting
            {
                Date = raitingFinMod.Date,
                Symbol = raitingFinMod.Symbol,
                Rating = _unitOfWork.Repository<Rating>().Find(rating => rating.Name == raitingFinMod.Rating).SingleOrDefault() ?? new Rating {Name = raitingFinMod.Rating},
                RatingRecommendation = GetRatingRecommendation(raitingFinMod.RatingRecommendation),
                RatingScore = raitingFinMod.RatingScore,
                RatingDetailsDCFRecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsDCFRecommendation),
                RatingDetailsDCFScore = raitingFinMod.RatingDetailsDCFScore,
                RatingDetailsDERecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsDERecommendation),
                RatingDetailsDEScore = raitingFinMod.RatingDetailsDEScore,
                RatingDetailsPBRecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsPBRecommendation),
                RatingDetailsPBScore = raitingFinMod.RatingDetailsPBScore,
                RatingDetailsPERecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsPERecommendation),
                RatingDetailsPEScore = raitingFinMod.RatingDetailsPEScore,
                RatingDetailsROARecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsROARecommendation),
                RatingDetailsROAScore = raitingFinMod.RatingDetailsROAScore,
                RatingDetailsROERecommendation = GetRatingRecommendation(raitingFinMod.RatingDetailsROERecommendation),
                RatingDetailsROEScore = raitingFinMod.RatingDetailsROEScore
            };
            return companyRaiting;
        }

        private static Dictionary<string, RatingRecommendation> _ratingRecommendations;
        private RatingRecommendation GetRatingRecommendation(string name)
        {
            if (_ratingRecommendations == null)
            {
                _ratingRecommendations = new Dictionary<string, RatingRecommendation>();
                foreach (RatingRecommendation ratingRecommendation in _unitOfWork.Repository<RatingRecommendation>().GetAll())
                {
                    _ratingRecommendations.Add(ratingRecommendation.Name, ratingRecommendation);
                }
            }

            if (!_ratingRecommendations.ContainsKey(name))
            {
                _ratingRecommendations.Add(name, new RatingRecommendation { Name = name } );
                _unitOfWork.Repository<RatingRecommendation>().Add(_ratingRecommendations[name]);
            }

            return _ratingRecommendations[name];
        }

        #endregion
    }
}
