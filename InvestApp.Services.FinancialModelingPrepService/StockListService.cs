using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Models.Models;
using InvestApp.Services.Interfaces;

namespace InvestApp.Services.FinancialModelingPrepService
{
    public class StockListService : IStockListService
    {
        private readonly FinancialModelingPrepHttpClientFactory _httpClientFactory;

        public StockListService(FinancialModelingPrepHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<StockListItem>> GetStockList()
        {
            using (FinancialModelingPrepHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = "stock/list";

                var stockList = await client.GetAsync<List<StockListItem>>(uri);

                return stockList;

            }
        }
    }
}