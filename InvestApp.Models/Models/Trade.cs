using System;
using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class Trade : BaseEntity
    {
        public string TradeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}