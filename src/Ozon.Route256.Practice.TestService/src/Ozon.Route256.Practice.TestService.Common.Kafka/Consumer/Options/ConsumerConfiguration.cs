using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class ConsumerConfiguration : ConsumerConfig
{
    public required string Topic { get; init; }
}
