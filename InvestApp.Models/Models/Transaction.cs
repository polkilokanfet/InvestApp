using System;
using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    /// <summary>
    /// Рыночная транзакция (покупка / продажа / дивиденды и т.д.)
    /// </summary>
    public class Transaction : BaseEntity
    {
        /// <summary>
        /// Id in Tinkoff Api
        /// </summary>
        public string IdTcs { get; set; }

        /// <summary>
        /// Инструмент транзакции
        /// </summary>
        public Instrument Instrument { get; set; }

        /// <summary>
        /// Количество в транзакции
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Тип транзакции (покупка / продажа / налог и т.д.)
        /// </summary>
        public ExtendedOperationType OperationType { get; set; }

        /// <summary>
        /// Статус транзакции
        /// </summary>
        public OperationStatus Status { get; set; }

        /// <summary>
        /// Комиссия за транзакцию
        /// </summary>
        public MoneySum Commission { get; set; } = new MoneySum();

        /// <summary>
        /// Цена единицы
        /// </summary>
        public MoneySum Price { get; set; } = new MoneySum();

        /// <summary>
        /// Дата транзакции
        /// </summary>
        public DateTime Date { get; set; }

        public bool IsMarginCall { get; set; } = false;
    }
}
