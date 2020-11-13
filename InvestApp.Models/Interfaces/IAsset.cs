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
        MarketInstrument MarketInstrument { get; }

        /// <summary>
        /// Количество
        /// </summary>
        int Shares { get; }

        /// <summary>
        /// Цена покупки единицы актива
        /// </summary>
        double PricePerShareBuy { get; }

        /// <summary>
        /// Цена текущая единицы актива
        /// </summary>
        double PricePerShare { get; }
    }
}