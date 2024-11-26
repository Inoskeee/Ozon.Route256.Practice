namespace Ozon.Route256.TestService.Integrations.Orders.Messages;

public class OrderInputErrorsMessage
{
    public required InputOrder InputOrder { get; set; }

    public required ErrorReason ErrorReason { get; set; }
}

public class InputOrder
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

public class ErrorReason
{
    public string Code { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}
