using System.Text.Json;
using Confluent.Kafka;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Serializers;

internal sealed class KafkaJsonSerializer<TMessage> : IDeserializer<TMessage>, ISerializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<TMessage>(data)!;
    }

    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
