namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Entities;

internal sealed class ErrorReason
{
    public string Code { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}