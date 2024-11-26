using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class MessageDeserializer<TKey, TValue> : IMessageDeserializer<TKey, TValue>
    where TKey : class
    where TValue : class
{
    private readonly IKafkaDeserializer<TKey> _keyDeserializer;
    private readonly IKafkaDeserializer<TValue> _valueDeserializer;

    public MessageDeserializer(
        IKafkaDeserializer<TKey> keyDeserializer,
        IKafkaDeserializer<TValue> valueDeserializer)
    {
        _keyDeserializer = keyDeserializer;
        _valueDeserializer = valueDeserializer;
    }

    public KafkaMessage<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> DeserializeMessage(ConsumeResult<byte[], byte[]> consumed)
    {
        var key = _keyDeserializer.Deserialize(
            consumed.Message.Key ?? Array.Empty<byte>(),
            new SerializationContext(MessageComponentType.Key, consumed.Topic, consumed.Message.Headers));

        var value = _valueDeserializer.Deserialize(
            consumed.Message.Value ?? Array.Empty<byte>(),
            new SerializationContext(MessageComponentType.Value, consumed.Topic, consumed.Message.Headers));

        return new KafkaMessage<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>>
        {
            Key = key,
            Value = value,
            Headers = consumed.Message.Headers,
            TopicPartitionOffset = consumed.TopicPartitionOffset,
            TimeStamp = consumed.Message.Timestamp.UtcDateTime,
        };
    }
}
