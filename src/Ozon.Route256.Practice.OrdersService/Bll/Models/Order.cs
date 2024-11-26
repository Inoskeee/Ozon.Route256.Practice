using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Bll.Models;

public record Order(
    long OrderId,
    long CustomerId,
    OrderStatus Status,
    Region Region,
    string Comment,
    DateTimeOffset CreatedAt);