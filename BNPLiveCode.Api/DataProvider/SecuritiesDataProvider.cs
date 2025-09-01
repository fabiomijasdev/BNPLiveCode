namespace BNPLiveCode.Api.DataProvider
{
    public class SecuritiesDataProvider : ISecuritiesDataProvider
    {
        private readonly HttpClient _httpClient;

        public SecuritiesDataProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<decimal> GetSecurityByIsinAsync(string isin)
        {
            var response = await _httpClient.GetAsync($"securityprice/{isin}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<decimal>();
        }
    }
}
