namespace Ozon.Route256.OrderService.Dal.Entities;

public class OrderInfoEntity
{
    public long OrderId { get; init; }

    public int Status { get; init; }

    public long CustomerId { get; init; }

    public long RegionId { get; init; }

    public string RegionName { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }

    public string Comment { get; init; } = string.Empty;

    public int TotalCount { get; init; }
}