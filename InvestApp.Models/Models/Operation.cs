using System;
using System.Collections.Generic;
using InvestApp.Models.Base;

namespace InvestApp.Models.Models
{
    public class Operation : BaseEntity
    {
        /// <summary>
        /// Id in Tinkoff Api
        /// </summary>
        public string IdTcs { get; set; }
        public OperationStatus Status { get; set; }
        public List<Trade> Trades { get; set; }
        public MoneyAmount Commission { get; set; }
        public Currency Currency { get; set; }
        public decimal Payment { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Figi { get; set; }
        public InstrumentType InstrumentType { get; set; }
        public bool IsMarginCall { get; set; }
        public DateTime Date { get; set; }
        public ExtendedOperationType OperationType { get; set; }
    }
}
