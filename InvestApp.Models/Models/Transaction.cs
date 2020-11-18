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
        /// Валюта транзакции
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Цена единицы
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Платеж транзакции
        /// </summary>
        public decimal Payment { get; set; }

        /// <summary>
        /// Дата транзакции
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Курс для перевода платежа к валюте инструмента
        /// </summary>
        public decimal? ExchangeRate { get; set; }

        public bool IsMarginCall { get; set; } = false;
    }
}
