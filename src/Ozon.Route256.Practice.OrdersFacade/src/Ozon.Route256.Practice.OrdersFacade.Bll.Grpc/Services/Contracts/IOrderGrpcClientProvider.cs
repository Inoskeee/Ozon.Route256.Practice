using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;

public interface IOrderGrpcClientProvider
{
    Task<List<V1QueryOrdersResponse>> GetOrdersByCustomerAsync(long customerId, int limit, int offset);
    Task<List<V1QueryOrdersResponse>> GetOrdersByRegionAsync(long regionId, int limit, int offset);
}