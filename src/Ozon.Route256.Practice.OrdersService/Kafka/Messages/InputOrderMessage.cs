namespace Ozon.Route256.OrderService.Kafka.Messages;

public class InputOrderMessage
{
    public long RegionId { get; set; }

    public long CustomerId { get; set; }

    public string? Comment { get; set; }

    public InputOrderItem[] Items { get; set; } = Array.Empty<InputOrderItem>();
}

public class InputOrderItem
{
    public string Barcode { get; set; } = string.Empty;

    public int Quantity { get; set; }
}