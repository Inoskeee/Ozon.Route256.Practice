using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.OrderGrpc;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Mappers;

internal interface IOrderMapper
{
    OrderModel MapOrderDtoToModel(V1QueryOrdersResponse ordersResponse);
}