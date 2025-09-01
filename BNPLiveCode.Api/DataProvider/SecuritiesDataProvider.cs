namespace BNPLiveCode.Api.DataProvider
{
    public class SecuritiesDataProvider : ISecuritiesDataProvider
    {
        private readonly HttpClient _httpClient;

        public SecuritiesDataProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public decimal GetSecurityByIsin(string isin)
        {
            return _httpClient.GetFromJsonAsync<decimal>($"url.com/{isin}").Result;
        }
    }
}
