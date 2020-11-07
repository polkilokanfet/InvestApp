namespace InvestApp.Services.FinancialModelingPrepService
{
    public class FinancialModelingHttpClientFactory
    {
        private readonly string _apiKey;

        public FinancialModelingHttpClientFactory(string apiKey)
        {
            _apiKey = apiKey;
        }

        public FinancialModelingHttpClient CreateHttpClient()
        {
            return new FinancialModelingHttpClient(_apiKey);
        }
    }
}
