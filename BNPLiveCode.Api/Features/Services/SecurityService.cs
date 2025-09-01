using BNPLiveCode.Api.DataProvider;
using BNPLiveCode.Api.Features.Models;
using BNPLiveCode.Api.Features.Repositories;

namespace BNPLiveCode.Api.Features.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly ISecuritiesDataProvider _securitiesDataProvider;

        public SecurityService(ISecurityRepository securityRepository, ISecuritiesDataProvider securitiesDataProvider, HttpClient httpClient)
        {
            _securityRepository = securityRepository;
            _securitiesDataProvider = securitiesDataProvider;
        }

        public async Task<List<Security>> RetriveIsinsAndSave(List<string> isins)
        {
            var securities = new List<Security>();

            foreach (var isin in isins)
            {
                var response = await _securitiesDataProvider.GetSecurityByIsinAsync(isin);

                var security = new Security
                {
                    Id = Guid.NewGuid(),
                    ISIN = isin,
                    Price = response
                };

                securities.Add(security);
            }
            await _securityRepository.AddOrUpdateBulkAsync(securities);
            return securities;
        }
    }
}
