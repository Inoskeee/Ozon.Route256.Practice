using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Services.Contracts;

public interface IOrderGrpcClientProvider
{
    Task<List<V1QueryOrdersResponse>> GetOrdersByCustomerAsync(
        long customerId, 
        CancellationToken cancellationToken);
}