using System;

using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class InstrumentInfoResponse : StreamingResponse<InstrumentInfoPayload>
    {
        public override string Event => "instrument_info";

        [JsonConstructor]
        public InstrumentInfoResponse(InstrumentInfoPayload payload, DateTime time)
            : base(payload, time)
        {
        }
    }
}