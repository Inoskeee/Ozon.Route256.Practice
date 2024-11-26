namespace Ozon.Route256.DataGenerator.Infra.Kafka.Settings;

public sealed record KafkaSettings
{
    public string? BootstrapServers { get; init; }
}
