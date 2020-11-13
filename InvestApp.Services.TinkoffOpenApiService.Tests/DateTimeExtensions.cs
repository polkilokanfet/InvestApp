using System;

namespace InvestApp.Services.TinkoffOpenApiService.Tests
{
    public static class DateTimeExtensions
    {
        public static DateTime AsUnspecified(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        }
    }
}
