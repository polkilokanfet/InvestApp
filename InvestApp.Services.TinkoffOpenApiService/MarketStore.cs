using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestApp.Domain.Interfaces;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services.DataBaseAccess;
using InvestApp.Services.TinkoffOpenApiService.Extensions;
using InvestApp.Services.TinkoffOpenApiService.Models;
using InvestApp.Services.TinkoffOpenApiService.Network;

namespace InvestApp.Services.TinkoffOpenApiService
{
    public class MarketStore : IMarketStore
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISandboxContext _context;

        public MarketStore(string tokenSandbox, IUnitOfWork unitOfWork)
        {
            var connection = ConnectionFactory.GetSandboxConnection(tokenSandbox);
            _context = connection.Context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Transaction>> RefreshTransactionsAsync()
        {
            List<Transaction> result = new List<Transaction>();

            //транзакции из базы данных
            List<Transaction> transactions = _unitOfWork.Repository<Transaction>().GetAllAsNoTracking();
            List<string> tcsIds = transactions.Select(transaction => transaction.IdTcs).Where(id => id != null).Distinct().ToList();

            //загрузка транзакций из Api
            IEnumerable<Operation> operationsApi = await _context.OperationsAsync(DateTime.Now.AddDays(-1000), DateTime.Now, null);
            List<Operation> operationsNew = operationsApi.Where(operation => !tcsIds.Contains(operation.Id)).ToList();

            if (operationsNew.Any())
            {
                List<Instrument> instruments = _unitOfWork.Repository<Instrument>().GetAll();
                List<string> figiList = instruments.Select(instrument => instrument.Figi).Where(figi => figi != null).Distinct().ToList();

                //обновление инструментов
                if (operationsNew.Select(operation => operation.Figi).Any(figi => !figiList.Contains(figi)))
                {
                    await RefreshMarketInstrumentsAsync();
                    instruments = _unitOfWork.Repository<Instrument>().GetAll();
                }

                foreach (Operation operation in operationsNew)
                {
                    Transaction transaction = operation.ToTransaction(instruments.Single(instrument => instrument.Figi == operation.Figi));
                    _unitOfWork.Repository<Transaction>().Add(transaction);
                    result.Add(transaction);
                }

                _unitOfWork.SaveChanges();
            }

            return result;
        }

        public IEnumerable<IAsset> GetAssets()
        {
            List<Transaction> transactions = _unitOfWork.Repository<Transaction>().GetAll();
            return transactions.GroupBy(transaction => transaction.Instrument).Select(x => new Asset(x));
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

        public async Task<decimal> GetPrice(string figi)
        {
            try
            {
                var orderBook = await _context.MarketOrderbookAsync(figi, 1);

                return orderBook.Asks.Any() 
                    ? orderBook.Asks.OrderBy(orderbookRecord => orderbookRecord.Price).First().Price 
                    : orderBook.LastPrice;
            }
            catch (OpenApiException e)
            {
                Console.WriteLine(e);
            }

            return 0;
        }

        public async Task SellPositionAsync(string figi, int lots)
        {
            MarketOrder marketOrder = new MarketOrder(figi, lots, OperationType.Sell);
            await _context.PlaceMarketOrderAsync(marketOrder);
        }
    }
}
