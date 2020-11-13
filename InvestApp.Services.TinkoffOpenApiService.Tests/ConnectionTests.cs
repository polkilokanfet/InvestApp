using System.Net.Http;
using FluentAssertions;
using InvestApp.Services.TinkoffOpenApiService.Network;
using Xunit;

namespace InvestApp.Services.TinkoffOpenApiService.Tests
{
    public class ConnectionTests
    {
        private class FakeConnection : Connection
        {
            public FakeConnection() : base("http://localhost", "http://localhost", "", new HttpClient())
            {
            }
        }

        [Fact]
        public void ShouldInitializeDefaults()
        {
            new FakeConnection().Defaults.Should().BeEquivalentTo(new Defaults());
        }
    }
}
