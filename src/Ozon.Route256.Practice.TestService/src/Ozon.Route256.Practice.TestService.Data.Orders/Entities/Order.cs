namespace Ozon.Route256.TestService.Data.Orders;

public class Order
{
    public long OrderId { get; init; }

    public long RegionId { get; init; }

    public long CustomerId { get; init; }

    public int Status { get; init; }

    public string? Comment { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public IReadOnlyCollection<OrderItem>? Items { get; internal set; }
}
