using System;
using System.Threading.Tasks;
using InvestApp.Models.Models;
using InvestApp.Services.Interfaces;

namespace InvestApp.Services.FinancialModelingPrepService
{
    public class MajorIndexService : IMajorIndexService
    {
        private readonly FinancialModelingPrepHttpClientFactory _httpClientFactory;

        public MajorIndexService(FinancialModelingPrepHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MajorIndex> GetMajorIndex(MajorIndexType indexType)
        {
            using (FinancialModelingPrepHttpClient client = _httpClientFactory.CreateHttpClient())
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