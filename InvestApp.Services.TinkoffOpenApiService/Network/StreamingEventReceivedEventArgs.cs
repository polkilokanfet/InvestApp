using InvestApp.Services.TinkoffOpenApiService.Models;

namespace InvestApp.Services.TinkoffOpenApiService.Network
{
    public class StreamingEventReceivedEventArgs
    {
        public StreamingResponse Response { get; }

        public StreamingEventReceivedEventArgs(StreamingResponse response)
        {
            Response = response;
        }
    }
}