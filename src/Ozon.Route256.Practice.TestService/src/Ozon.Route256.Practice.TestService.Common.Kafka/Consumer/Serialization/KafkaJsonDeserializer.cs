using System.Text.Json;
using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class KafkaJsonDeserializer<TPayload> : IKafkaDeserializer<TPayload>
    where TPayload : class
{
    private const string Format = "Json";

    private readonly JsonSerializerOptions _serializerOptions;

    public KafkaJsonDeserializer(JsonSerializerOptions? serializerOptions = null)
    {
        _serializerOptions = serializerOptions ?? KafkaJsonSerializer.DefaultSettings;
    }

    public KafkaMessagePart<TPayload> Deserialize(byte[] data, SerializationContext context)
    {
        var value = new KafkaMessagePart<TPayload>
        {
            RawValue = data,
            Format = Format
        };

        try
        {
            value.Payload = JsonSerializer.Deserialize<TPayload>(data, _serializerOptions);
        }
        catch (JsonException ex)
        {
            value.DeserializationException = ex;
        }

        return value;
    }
}
