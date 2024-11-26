namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Entities;

internal sealed class InputOrderItem
{
    public string Barcode { get; set; } = string.Empty;

    public int Quantity { get; set; }
}