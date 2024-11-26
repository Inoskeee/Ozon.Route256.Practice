using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services;

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

    public async Task<List<V1QueryOrdersResponse>> GetOrdersByCustomerAsync(long customerId, int limit, int offset)
    {
        var request = new V1QueryOrdersRequest()
        {
            RegionIds = {  },
            OrderIds = {  },
            CustomerIds = { customerId },
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

    public async Task<List<V1QueryOrdersResponse>> GetOrdersByRegionAsync(long regionId, int limit, int offset)
    {
        var request = new V1QueryOrdersRequest()
        {
            RegionIds = { regionId },
            OrderIds = {  },
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