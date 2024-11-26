namespace Ozon.Route256.OrderService.Dal.Entities;

public class ItemEntity
{
    public long Id { get; init; }
    
    public long OrderId { get; init; }

    public string Barcode { get; init; } = string.Empty;

    public int Quantity { get; init; }
}