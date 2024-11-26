namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

internal sealed record OrderEntity(
    long OrderId,
    long CustomerId,
    int OrderStatus,
    DateTime CreatedAt);