using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestApp.Domain.Exceptions;
using InvestApp.Domain.Models;
using InvestApp.Domain.Services;

namespace InvestApp.Services.FinancialModelingPrepService
{
    public class FinancialModelingService : IFinancialModelingPrepService
    {
        private readonly FinancialModelingHttpClientFactory _httpClientFactory;

        public FinancialModelingService(FinancialModelingHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<double> GetPrice(string symbol)
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = "stock/real-time-price/" + symbol;
                StockPriceResult stockPriceResult = await client.GetAsync<StockPriceResult>(uri);
                if (stockPriceResult.Price == 0)
                {
                    throw new InvalidSymbolException(symbol);
                }
                return stockPriceResult.Price;
            }
        }

        public async Task<List<StockListItem>> GetStockList()
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = "stock/list";
                var stockList = await client.GetAsync<List<StockListItem>>(uri);
                return stockList;
            }
        }

        public async Task<MajorIndex> GetMajorIndex(MajorIndexType indexType)
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = "majors-indexes/" + GetUriSuffix(indexType);
                MajorIndex majorIndex = await client.GetAsync<MajorIndex>(uri);
                majorIndex.Type = indexType;
                return majorIndex;
            }
        }

        private string GetUriSuffix(MajorIndexType indexType)
        {
            switch (indexType)
            {
                case MajorIndexType.DowJones: return ".DJI";
                case MajorIndexType.Nasdaq: return ".IXIC";
                case MajorIndexType.SP500: return ".INX";
                default: throw new Exception("MajorIndexType does not have a suffix defined.");
            }
        }
    }
}