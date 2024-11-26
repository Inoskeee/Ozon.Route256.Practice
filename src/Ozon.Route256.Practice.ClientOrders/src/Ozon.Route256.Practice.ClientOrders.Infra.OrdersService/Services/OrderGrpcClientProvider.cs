using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Services;
using Ozon.Route256.Practice.ClientOrders.OrderGrpc;

namespace Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Services;

internal sealed class OrderGrpcClientProvider : IOrderGrpcClientProvider
{
    private readonly ILogger<OrderGrpcClientProvider> _logger;
    private readonly OrderGrpc.OrderGrpc.OrderGrpcClient _orderGrpcClient;

    public OrderGrpcClientProvider(
        ILogger<OrderGrpcClientProvider> logger,
        OrderGrpc.OrderGrpc.OrderGrpcClient orderGrpcClient)
    {
        _orderGrpcClient = orderGrpcClient;
        _logger = logger;
    }
    

    public async Task<List<V1QueryOrdersResponse>> GetOrderByIdAsync(long orderId, int limit, int offset)
    {
        var request = new V1QueryOrdersRequest()
        {
            RegionIds = {  },
            OrderIds = { orderId },
            CustomerIds = { },
            Limit = limit,
            Offset = offset
        };
        var responseStream = _orderGrpcClient.V1QueryOrders(request);
        var orders = new List<V1QueryOrdersResponse>();
        while (await responseStream.ResponseStream.MoveNext())
        {
            var currentOrder = responseStream.ResponseStream.Current;
            orders.Add(currentOrder);
        }
        
        return orders;
    }
}