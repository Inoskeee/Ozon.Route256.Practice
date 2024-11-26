namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Entities;

internal sealed class InputOrder
{
    public long RegionId { get; set; }

    public long CustomerId { get; set; }

    public string? Comment { get; set; }

    public InputOrderItem[] Items { get; set; } = Array.Empty<InputOrderItem>();
}