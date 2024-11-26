using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IMessageDeserializer<TKey, TValue>
    where TKey : class
    where TValue : class
{
    KafkaMessage<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> DeserializeMessage(ConsumeResult<byte[], byte[]> consumed);
}
