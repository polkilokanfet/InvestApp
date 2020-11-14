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

    }
}