namespace Ozon.Route256.TestService.Data.Orders;

public class OrderItem
{
    public long Id { get; init; }

    public long OrderId { get; init; }

    public string Barcode { get; init; } = string.Empty;

    public int Quantity { get; init; }
}
