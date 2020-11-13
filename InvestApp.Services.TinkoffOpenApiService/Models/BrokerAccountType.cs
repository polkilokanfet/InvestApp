using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrokerAccountType
    {
        Tinkoff = 1,
        TinkoffIis = 2
    }
}