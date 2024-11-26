using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Bll.Models.Enums;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.OrderGrpc;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Mappers;

internal sealed class OrderMapper : IOrderMapper
{
    public OrderModel MapOrderDtoToModel(V1QueryOrdersResponse ordersResponse)
    {
        return new OrderModel
        {
            OrderId = ordersResponse.OrderId,
            CustomerId = ordersResponse.CustomerId,
            RegionId = ordersResponse.Region.Id,
            Comment = ordersResponse.Comment,
            Status = (OrderStatusEnum)ordersResponse.Status,
            CreatedAt = ordersResponse.CreatedAt.ToDateTime()
        };
    }
}