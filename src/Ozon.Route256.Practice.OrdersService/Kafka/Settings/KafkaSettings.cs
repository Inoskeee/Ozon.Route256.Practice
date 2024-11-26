namespace Ozon.Route256.OrderService.Kafka.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;

    public string GroupId { get; set; } = string.Empty;

    public int TimeoutForRetryInSeconds { get; set; } = 2;
}