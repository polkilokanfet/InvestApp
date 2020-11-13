using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class OpenApiExceptionPayload
    {
        public string Message { get; }
        public string Code { get; }

        [JsonConstructor]
        public OpenApiExceptionPayload(string message, string code)
        {
            this.Message = message;
            this.Code = code;
        }
    }
}