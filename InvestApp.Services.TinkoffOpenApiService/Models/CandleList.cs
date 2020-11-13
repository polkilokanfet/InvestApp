using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class CandleList
    {
        public string Figi { get; }
        public CandleInterval Interval { get; }
        public List<CandlePayload> Candles { get; }
        
        [JsonConstructor]
        public CandleList(string figi, CandleInterval interval, List<CandlePayload> candles)
        {
            Figi = figi;
            Interval = interval;
            Candles = candles;
        }
    }
}