using BNPLiveCode.Api.Features.Models;

namespace BNPLiveCode.Api.Features.Services;

public interface ISecurityService
{
    Task<List<Security>> RetriveIsinsAndSave(List<string> isins);
}