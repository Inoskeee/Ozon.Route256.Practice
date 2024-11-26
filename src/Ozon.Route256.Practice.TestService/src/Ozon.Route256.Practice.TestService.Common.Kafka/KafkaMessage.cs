using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka;

public class KafkaMessage<TKey, TValue>
    where TKey : class
    where TValue : class
{
    public TKey Key { get; init; } = null!;

    public TValue Value { get; init; } = null!;

    public DateTime TimeStamp { get; init; }

    public Headers Headers { get; init; } = null!;

    public TopicPartitionOffset TopicPartitionOffset { get; init; } = null!;
}
