using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;

public interface IOrderHttpClientProvider
{
    Task<List<OrdersHttpResponseDto>> GetOrdersByCustomerAsync(long customerId, int limit, int offset);
    Task<List<OrdersHttpResponseDto>> GetOrdersByRegionAsync(long regionId, int limit, int offset);
}