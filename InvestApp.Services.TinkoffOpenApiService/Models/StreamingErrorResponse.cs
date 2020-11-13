using System;

using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class StreamingErrorResponse : StreamingResponse<StreamingErrorPayload>
    {
        public override string Event => "error";

        [JsonConstructor]
        public StreamingErrorResponse(StreamingErrorPayload payload, DateTime time)
            : base(payload, time)
        {
        }
    }
}