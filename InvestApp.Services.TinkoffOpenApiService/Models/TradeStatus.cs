using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TradeStatus
    {
        NormalTrading,
        NotAvailableForTrading
    }
}