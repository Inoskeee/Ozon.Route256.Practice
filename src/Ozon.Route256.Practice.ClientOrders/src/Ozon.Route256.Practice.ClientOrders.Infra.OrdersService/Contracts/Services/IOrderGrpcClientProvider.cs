using Ozon.Route256.Practice.ClientOrders.OrderGrpc;

namespace Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Services;

public interface IOrderGrpcClientProvider
{
    Task<List<V1QueryOrdersResponse>> GetOrderByIdAsync(long orderId, int limit, int offset);
}