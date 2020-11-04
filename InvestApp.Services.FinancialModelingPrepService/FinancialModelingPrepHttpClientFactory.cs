namespace InvestApp.Services.FinancialModelingPrepService
{
    public class FinancialModelingPrepHttpClientFactory
    {
        private readonly string _apiKey;

        public FinancialModelingPrepHttpClientFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public FinancialModelingPrepHttpClient CreateHttpClient()
        {
            return new FinancialModelingPrepHttpClient(_apiKey);
        }
    }
}
