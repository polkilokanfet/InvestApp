using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class Exchange : BaseEntity
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}