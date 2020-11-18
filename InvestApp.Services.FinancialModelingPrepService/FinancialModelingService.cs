using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestApp.Domain.Exceptions;
using InvestApp.Domain.Models;
using InvestApp.Domain.Models.FinMod;
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

        public async Task<double> GetPriceAsync(string symbol)
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

        public async Task<List<StockListItem>> GetStockListAsync()
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = "stock/list";
                List<StockListItem> stockList = await client.GetAsync<List<StockListItem>>(uri);
                return stockList;
            }
        }

        public async Task<CompanyProfileFinMod> GetCompanyProfileAsync(string symbol)
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = $"profile/{symbol}";
                List<CompanyProfileFinMod> companyProfiles = await client.GetAsync<List<CompanyProfileFinMod>>(uri);
                if (!companyProfiles.Any())
                {
                    throw new InvalidSymbolException(symbol);
                }
                return companyProfiles.First();
            }
        }

        public async Task<List<FinancialRatio>> GetFinancialRatiosAsync(string symbol)
        {
            using (FinancialModelingHttpClient client = _httpClientFactory.CreateHttpClient())
            {
                string uri = $"ratios/{symbol}";
                List<FinancialRatio> ratios = await client.GetAsync<List<FinancialRatio>>(uri);
                return ratios;
            }
        }

        #region GetMajorIndex

        public async Task<MajorIndex> GetMajorIndexAsync(MajorIndexType indexType)
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

        #endregion
    }
}