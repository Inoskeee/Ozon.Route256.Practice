using RestSharp;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;

public interface IBalancedRestClient
{
    Task<RestResponse?> ExecuteAsync(RestRequest request, string endpoint);
}