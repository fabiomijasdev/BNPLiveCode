using BNPLiveCode.Api.Data;
using BNPLiveCode.Api.Features.Models;
using Microsoft.EntityFrameworkCore;

namespace BNPLiveCode.Api.Features.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly AppDbContext _dbContext;
        public SecurityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddOrUpdateBulkAsync(IEnumerable<Security> securities)
        {
            var isins = securities.Select(s => s.ISIN).ToList();

            var existingSecurities = await _dbContext
                .Securities
                .Where(s => isins.Contains(s.ISIN))
                .ToListAsync();

            foreach (var security in securities)
            {
                var existing = existingSecurities.FirstOrDefault(s => s.ISIN == security.ISIN);

                if (existing == null)
                {
                    await _dbContext.Securities.AddAsync(security);
                }
                else
                {
                    existing.Price = security.Price;
                    _dbContext.Securities.Update(existing);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
