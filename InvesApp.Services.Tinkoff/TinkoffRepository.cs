using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using InstrumentType = InvestApp.Domain.Models.InstrumentType;
using Operation = InvestApp.Domain.Models.Operation;
using OperationTcs = Tinkoff.Trading.OpenApi.Models.Operation;

namespace InvesApp.Services.Tinkoff
{
    public class TinkoffRepository 
    {
        private readonly ISandboxContext _context;

        public TinkoffRepository()
        {
            var connection = ConnectionFactory.GetSandboxConnection("t.6S_c2O6mXa8dkH6aHkZvACVa_jeRyRAR-q_k7zmF-0qbyjW2vEX09a01wCJegeJ6aV38vPfl5jsvw_taUpbuUQ");
            _context = connection.Context;
        }

        public async Task<Portfolio> GetPortfolioAsync()
        {
            return await _context.PortfolioAsync();
        }

        public async Task<List<OperationTcs>> GetOperationsAllAsync()
        {
            return await _context.OperationsAsync(DateTime.Now.AddDays(-700), DateTime.Now, null);
        }

        public async Task<List<Operation>> GetOperationsAsync()
        {
            var result = new List<Operation>();

            var account = await _context.RegisterAsync(BrokerAccountType.Tinkoff);
            var portfolio = await this.GetPortfolioAsync();
            foreach (var position in portfolio.Positions)
            {
                var operations = await _context.OperationsAsync(DateTime.Now.AddDays(-30), DateTime.Now, position.Figi);
                foreach (var operation in operations)
                {
                    result.Add(new Operation()
                    {
                        Figi = operation.Figi,
                        Payment = operation.Payment,
                        Price = operation.Price,
                        Date = operation.Date,
                        InstrumentType = (InstrumentType)((int)operation.InstrumentType)
                    });
                }
            }

            return result;
        }

        public async Task<List<MarketInstrument>> GetMarketInstruments()
        {
            var stocks = (await _context.MarketStocksAsync()).Instruments;
            var bonds = (await _context.MarketBondsAsync()).Instruments;
            var etfs = (await _context.MarketEtfsAsync()).Instruments;
            var currencies = (await _context.MarketCurrenciesAsync()).Instruments;

            List<MarketInstrument> result = new List<MarketInstrument>();
            result.AddRange(stocks);
            result.AddRange(bonds);
            result.AddRange(etfs);
            result.AddRange(currencies);

            return result;
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
