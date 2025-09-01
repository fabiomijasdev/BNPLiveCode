namespace BNPLiveCode.Api.DataProvider;

public interface ISecuritiesDataProvider
{
    Task<decimal> GetSecurityByIsinAsync(string isin);
}