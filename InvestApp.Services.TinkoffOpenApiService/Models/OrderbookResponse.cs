using System;

using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class OrderbookResponse : StreamingResponse<OrderbookPayload>
    {
        public override string Event => "orderbook";

        [JsonConstructor]
        public OrderbookResponse(OrderbookPayload payload, DateTime time)
            : base(payload, time)
        {
        }
    }
}