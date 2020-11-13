using System;

using FluentAssertions;
using InvestApp.Services.TinkoffOpenApiService.Tests.TestHelpers;
using Newtonsoft.Json;

using InvestApp.Services.TinkoffOpenApiService.Models;
using InvestApp.Services.TinkoffOpenApiService.Tests.TestHelpers;

using Xunit;

namespace InvestApp.Services.TinkoffOpenApiService.Tests
{
    public class StreamingResponseTests
    {
        [Fact]
        public void DeserializeCandleTest()
        {
            var streamingResponse = JsonConvert.DeserializeObject<StreamingResponse>(JsonFile.Read("streaming-candle-response"));
            var response = streamingResponse as StreamingResponse<CandlePayload>;

            var expectedResponse = new CandleResponse(
                new CandlePayload(
                    64.0575m,
                    64.0578m,
                    64.0579m,
                    64.0573m,
                    156,
                    new DateTime(2019, 08, 07, 15, 35, 00, DateTimeKind.Utc),
                    CandleInterval.FiveMinutes,
                    "BBG0013HGFT4"),
                new DateTime(2019, 08, 07, 15, 35, 01, 029, DateTimeKind.Utc).AddTicks(7213));

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
