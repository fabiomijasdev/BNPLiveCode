using BNPLiveCode.Api.Features.Models;
using Microsoft.EntityFrameworkCore;

namespace BNPLiveCode.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Security> Securities { get; set; }
    }
}
