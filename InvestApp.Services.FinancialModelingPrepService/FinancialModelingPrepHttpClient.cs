using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InvestApp.Services.FinancialModelingPrepService
{
    public class FinancialModelingPrepHttpClient : HttpClient
    {
        private readonly string _apiKey;

        public FinancialModelingPrepHttpClient(string apiKey)
        {
            this.BaseAddress = new Uri("https://financialmodelingprep.com/api/v3/");
            _apiKey = apiKey;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            HttpResponseMessage response = await GetAsync($"{uri}?apikey={_apiKey}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}