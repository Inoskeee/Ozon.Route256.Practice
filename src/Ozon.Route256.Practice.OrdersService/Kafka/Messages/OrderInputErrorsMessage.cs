namespace Ozon.Route256.OrderService.Kafka.Messages;

public class OrderInputErrorsMessage
{
    public InputOrder InputOrder { get; set; }

    public ErrorReason ErrorReason { get; set; }
}

public class InputOrder
{
    public long RegionId { get; set; }

    public long CustomerId { get; set; }

    public string? Comment { get; set; }

    public InputOrderItem[] Items { get; set; } = Array.Empty<InputOrderItem>();
}

public class ErrorReason
{
    public string Code { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}