using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class MarketInstrumentList
    {
        public int Total { get; }
        public List<MarketInstrument> Instruments { get; }

        [JsonConstructor]
        public MarketInstrumentList(int total, List<MarketInstrument> instruments)
        {
            Total = total;
            Instruments = instruments;
        }
    }
}