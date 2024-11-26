using Ozon.Route256.Practice.ClientOrders.Bll.Models.Enums;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;

public sealed record OrderModel(
    long OrderId,
    long CustomerId,
    OrderStatusEnum OrderStatus,
    DateTimeOffset CreatedAt);