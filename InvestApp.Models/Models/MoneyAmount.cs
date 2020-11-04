using InvestApp.Models.Base;

namespace InvestApp.Models.Models
{
    public class MoneyAmount : BaseEntity
    {
        public Currency Currency { get; set; }
        public decimal Value { get; set; }
    }
}