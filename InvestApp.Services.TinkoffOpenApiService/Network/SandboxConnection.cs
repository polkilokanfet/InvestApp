using System.Net.Http;

namespace InvestApp.Services.TinkoffOpenApiService.Network
{
    public class SandboxConnection : Connection<SandboxContext>
    {
        public SandboxConnection(string baseUri, string webSocketBaseUri, string token, HttpClient httpClient)
            : base(baseUri, webSocketBaseUri, token, httpClient)
        {
        }

        public override SandboxContext Context => new SandboxContext(this);
    }
}