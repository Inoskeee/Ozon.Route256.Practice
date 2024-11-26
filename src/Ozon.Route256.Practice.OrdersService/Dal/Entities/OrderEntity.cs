namespace Ozon.Route256.OrderService.Dal.Entities;

public class OrderEntity
{
    public long OrderId { get; init; }

    public long RegionId { get; init; }

    public long CustomerId { get; init; }

    public int Status { get; init; }

    public string? Comment { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}