using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IKafkaDeserializer<T>
    where T : class
{
    KafkaMessagePart<T> Deserialize(byte[] data, SerializationContext context);
}
