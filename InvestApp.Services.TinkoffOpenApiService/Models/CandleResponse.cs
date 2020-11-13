using System;

using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class CandleResponse : StreamingResponse<CandlePayload>
    {
        public override string Event => "candle";

        [JsonConstructor]
        public CandleResponse(CandlePayload payload, DateTime time)
            : base(payload, time)
        {
        }
    }
}