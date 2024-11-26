namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Entities;

internal sealed class OrderInputErrorsMessage
{
    public InputOrder InputOrder { get; set; }

    public ErrorReason ErrorReason { get; set; }
}