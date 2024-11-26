using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class KafkaByteArrayDeserializer : IKafkaDeserializer<byte[]>
{
    private const string Format = "ByteArray";

    public KafkaMessagePart<byte[]> Deserialize(byte[] data, SerializationContext context)
    {
        return new KafkaMessagePart<byte[]>
        {
            Format = Format,
            Payload = data,
            RawValue = data
        };
    }
}
