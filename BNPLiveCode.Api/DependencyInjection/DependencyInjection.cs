using BNPLiveCode.Api.DataProvider;
using BNPLiveCode.Api.Features.Repositories;
using BNPLiveCode.Api.Features.Services;

namespace BNPLiveCode.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISecurityRepository, SecurityRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();
        }

        public static void AddSecuritiesDataProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["SecuritiesDataProvider:BaseUrl"];
            services.AddHttpClient<ISecuritiesDataProvider, SecuritiesDataProvider>(client =>
            {
                client.BaseAddress = new Uri(baseUrl!);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
