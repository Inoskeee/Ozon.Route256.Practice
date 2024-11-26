using Confluent.Kafka;

namespace Ozon.Route256.DataGenerator.Infra.Kafka.Settings;

public record ProducerSettings
{
    public required Acks Acks { get; init; } = Acks.Leader;

    public required bool EnableIdempotence { get; init; }

    public required double LingerMs { get; init; }
}
