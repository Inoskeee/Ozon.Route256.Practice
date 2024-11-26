using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;

public interface ICustomerHttpClientProvider
{
    Task<CustomersHttpResponseDto> GetCustomersAsync(long[] customerIds, int limit, int offset);
}