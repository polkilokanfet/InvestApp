using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InvestApp.Domain.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationStatus
    {
        Done,
        Decline,
        Progress
    }
}