using BNPLiveCode.Api.Features.Models;

namespace BNPLiveCode.Api.Features.Repositories
{
    public interface ISecurityRepository
    {
        Task AddOrUpdateBulkAsync(IEnumerable<Security> securities);
    }
}
