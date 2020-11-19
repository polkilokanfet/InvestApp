using System.Linq;
using InvestApp.Domain.Models;
using InvestApp.Domain.Models.FinMod;
using InvestApp.Domain.Services.DataBaseAccess;
using InvestApp.Services.TinkoffOpenApiService.Models;

namespace InvestApp.Services.TinkoffOpenApiService.Extensions
{
    public static class ConverterExtansions
    {
        public static MoneySum ToMoneySum(this MoneyAmount moneyAmount)
        {
            return moneyAmount == null
                ? null
                : new MoneySum
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

        public static CompanyProfile ToCompanyProfile(this CompanyProfileFinMod companyProfileFinMod, IUnitOfWork unitOfWork)
        {
            return new CompanyProfile
            {
                Cik = companyProfileFinMod.Cik,
                Cusip = companyProfileFinMod.Cusip,
                Currency = companyProfileFinMod.Currency,
                CompanyName = companyProfileFinMod.CompanyName,
                Description = companyProfileFinMod.Description,
                Country = unitOfWork.Repository<Country>().Find(country => country.Name == companyProfileFinMod.Country).SingleOrDefault() ?? new Country { Name = companyProfileFinMod.Country },
                Exchange = unitOfWork.Repository<Exchange>().Find(exchange => exchange.ShortName == companyProfileFinMod.ExchangeShortName).SingleOrDefault() ?? new Exchange { ShortName = companyProfileFinMod.ExchangeShortName, FullName = companyProfileFinMod.Exchange },
                Industry = unitOfWork.Repository<Industry>().Find(industry => industry.Name == companyProfileFinMod.Industry).SingleOrDefault() ?? new Industry { Name = companyProfileFinMod.Industry },
                Sector = unitOfWork.Repository<Sector>().Find(sector => sector.Name == companyProfileFinMod.Sector).SingleOrDefault() ?? new Sector { Name = companyProfileFinMod.Sector },
                Image = companyProfileFinMod.Image,
                IpoDate = companyProfileFinMod.IpoDate,
                Isin = companyProfileFinMod.Isin,
                Symbol = companyProfileFinMod.Symbol,
                Website = companyProfileFinMod.Website
            };
        }
    }
}