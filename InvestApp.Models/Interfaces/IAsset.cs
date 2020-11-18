using System;
using InvestApp.Domain.Models;

namespace InvestApp.Domain.Interfaces
{
    /// <summary>
    /// Актив
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Инструмент
        /// </summary>
        Instrument Instrument { get; }

        /// <summary>
        /// Количество
        /// </summary>
        int Shares { get; }

        /// <summary>
        /// Цена покупки единицы актива
        /// </summary>
        decimal PricePerShareBuy { get; }

        /// <summary>
        /// Цена текущая единицы актива
        /// </summary>
        decimal PricePerShare { get; }

        /// <summary>
        /// Событие изменения последнего прайса инструмента
        /// </summary>
        event Action<Instrument, decimal> InstrumentLastPriceChanged;

        /// <summary>
        /// Событие изменения коэффициента приведения транзакции к валюте инструмента
        /// </summary>
        event Action<Transaction, decimal> TransactionExchangeRateChanged;
    }
}