namespace BNPLiveCode.Api.DataProvider;

public interface ISecuritiesDataProvider
{
    decimal GetSecurityByIsin(string isins);
}