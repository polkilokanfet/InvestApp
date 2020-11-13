using Newtonsoft.Json;

namespace InvestApp.Services.TinkoffOpenApiService.Models
{
    public class OrderbookRecord
    {
        public int Quantity { get; }
        public decimal Price { get; }

        [JsonConstructor]
        public OrderbookRecord(int quantity, decimal price)
        {
            Quantity = quantity;
            Price = price;
        }
    }
}