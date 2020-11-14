using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Models;

namespace InvestApp.Domain.Interfaces
{
    public interface IMarketStore
    {
        /// <summary>
        /// Обновить рыночные транзакции через API
        /// </summary>
        /// <returns>Транзакции, которые не были сохранены</returns>
        Task<IEnumerable<Transaction>> RefreshTransactionsAsync();

        /// <summary>
        /// Получить все активы
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAsset> GetAssets();

        /// <summary>
        /// Получить все возможные рыночные инструменты
        /// </summary>
        /// <returns></returns>
        Task<List<Instrument>> GetMarketInstruments();

        /// <summary>
        /// Обновить рыночные инструменты через API
        /// </summary>
        /// <param name="includeRefreshPrices">Включая обновление последней цены</param>
        /// <returns>Инструменты, которые не были сохранены.</returns>
        Task<List<Instrument>> RefreshMarketInstrumentsAsync(bool includeRefreshPrices = true);

        Task<decimal> RefreshInstrumentLastPriceAsync(string figi);
    }
}