using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Grpc;

public class OrderGrpcService : OrderGrpc.OrderGrpcBase
{
    private readonly IOrderService _orderService;

    public OrderGrpcService(
        IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public override async Task V1QueryOrders(
        V1QueryOrdersRequest request,
        IServerStreamWriter<V1QueryOrdersResponse> responseStream,
        ServerCallContext context)
    {
        var (orders, totalCount) = await _orderService.QueryOrders(
            request.OrderIds.ToArray(),
            request.CustomerIds.ToArray(),
            request.RegionIds.ToArray(),
            request.Limit,
            request.Offset,
            context.CancellationToken);

        foreach (var order in orders)
        {
            await responseStream.WriteAsync(
                new V1QueryOrdersResponse
                {
                    TotalCount = totalCount,
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    Region = new V1QueryOrdersResponse.Types.Region
                    {
                        Id = order.Region.Id,
                        Name = order.Region.Name
                    },
                    Status = order.Status,
                    Comment = order.Comment,
                    CreatedAt = order.CreatedAt.ToTimestamp()
                });
        }
    }

    public override async Task<V1CancelOrderResponse> V1CancelOrder(
        V1CancelOrderRequest request,
        ServerCallContext context)
    {
        var result = await _orderService.CancelOrder(request.OrderId, context.CancellationToken);
        return result.Success
            ? new V1CancelOrderResponse { Ok = new V1CancelOrderResponse.Types.Success() }
            : new V1CancelOrderResponse
            {
                Error = new V1CancelOrderResponse.Types.Error
                {
                    Code = result.Error?.ErrorCode,
                    Text = result.Error?.ErrorMessage
                }
            };
    }

    public override async Task<V1DeliveryOrderResponse> V1DeliveryOrder(
        V1DeliveryOrderRequest request,
        ServerCallContext context)
    {
    
        var result = await _orderService.DeliveryOrder(request.OrderId, context.CancellationToken);

        return result.Success
            ? new V1DeliveryOrderResponse { Ok = new V1DeliveryOrderResponse.Types.DeliverySuccess() }
            : new V1DeliveryOrderResponse
            {
                Error = new V1DeliveryOrderResponse.Types.DeliveryError
                {
                    Code = result.Error?.ErrorCode,
                    Text = result.Error?.ErrorMessage
                }
            };
    }
}
