using Confluent.Kafka;
using Google.Protobuf;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class KafkaProtoDeserializer<T> : IKafkaDeserializer<T>
    where T : class, IMessage<T>, new()
{
    private const string Format = "Protobuf";

    private readonly MessageParser<T> _parser = new(() => new T());

    public KafkaMessagePart<T> Deserialize(byte[] data, SerializationContext context)
    {
        var result = new KafkaMessagePart<T>
        {
            Format = Format,
            RawValue = data.ToArray(),
        };

        if (data.Length == 0)
            return result;

        try
        {
            result.Payload = _parser.ParseFrom(data);
        }
        catch (InvalidProtocolBufferException ex)
        {
            result.DeserializationException = ex;
        }

        return result;
    }
}
