using System.Linq;
using InvestApp.Domain.Models;
using InvestApp.Services.TinkoffOpenApiService.Models;

namespace InvestApp.Services.TinkoffOpenApiService.Extensions
{
    public static class ConverterExtansions
    {
        public static MoneySum ToMoneySum(this MoneyAmount moneyAmount)
        {
            return moneyAmount == null
                ? null
                : new MoneySum()
                {
                    Currency = moneyAmount.Currency,
                    Value = moneyAmount.Value
                };
        }

        public static Instrument ToInstrument(this MarketInstrument marketInstrument)
        {
            if (marketInstrument == null)
                return null;

            return new Instrument
            {
                Figi = marketInstrument.Figi,
                Isin = marketInstrument.Isin,
                Ticker = marketInstrument.Ticker,
                Lot = marketInstrument.Lot,
                MinPriceIncrement = marketInstrument.MinPriceIncrement,
                Name = marketInstrument.Name,
                Type = marketInstrument.Type,
                Currency = marketInstrument.Currency
            };

        }

        public static Transaction ToTransaction(this Operation operation, Instrument instrument)
        {
            return new Transaction
            {
                IdTcs = operation.Id,
                Date = operation.Date,
                IsMarginCall = operation.IsMarginCall,
                OperationType = operation.OperationType,
                Status = operation.Status,
                Quantity = operation.Trades?.Sum(trade => trade.Quantity) ?? operation.Quantity,
                Commission = operation.Commission.ToMoneySum(),
                Currency = operation.Currency,
                Price = operation.Price,
                Payment = operation.Payment,
                Instrument = instrument
            };

        }
    }
}