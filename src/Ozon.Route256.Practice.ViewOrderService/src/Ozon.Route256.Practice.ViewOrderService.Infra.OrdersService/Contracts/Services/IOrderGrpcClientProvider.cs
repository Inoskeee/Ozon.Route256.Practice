using Ozon.Route256.Practice.ViewOrderService.OrderGrpc;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Services;

internal interface IOrderGrpcClientProvider
{
    Task<List<V1QueryOrdersResponse>> GetOrderByIdAsync(long orderId, int limit, int offset);
}