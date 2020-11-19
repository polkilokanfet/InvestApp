using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    /// <summary>
    /// Маркер того, что в сервисе нет информации по указанной бумаге (тикеру)
    /// </summary>
    public class FinancialModelingServiceInvalidSymbols : BaseEntity
    {
        public string InvalidSymbols { get; set; }
    }
}