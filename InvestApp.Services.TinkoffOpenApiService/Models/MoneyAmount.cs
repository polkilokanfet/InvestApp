using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class MoneyAmount
    {
        public Currency Currency { get; }
        public decimal Value { get; }

        [JsonConstructor]
        public MoneyAmount(Currency currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }
    }
}