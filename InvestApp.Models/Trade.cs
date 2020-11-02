using System;

namespace InvestApp.Models
{
    public class Trade
    {
        public string TradeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}