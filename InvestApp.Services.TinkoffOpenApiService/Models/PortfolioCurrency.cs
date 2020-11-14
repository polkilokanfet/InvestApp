using System.Collections.Generic;
using InvestApp.Domain.Models;
using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class PortfolioCurrencies
    {
        public List<PortfolioCurrency> Currencies { get; }

        [JsonConstructor]
        public PortfolioCurrencies(List<PortfolioCurrency> currencies)
        {
            Currencies = currencies;
        }

        public class PortfolioCurrency
        {
            public Currency Currency { get; }
            public decimal Balance { get; }
            public decimal Blocked { get; }

            [JsonConstructor]
            public PortfolioCurrency(Currency currency, decimal balance, decimal blocked)
            {
                Currency = currency;
                Balance = balance;
                Blocked = blocked;
            }
        }
    }
}