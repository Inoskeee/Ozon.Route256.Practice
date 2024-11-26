using Confluent.Kafka;
using Google.Protobuf;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Serializers;

internal class KafkaProtoSerializer<TMessage> : IDeserializer<TMessage>, ISerializer<TMessage> where TMessage : IMessage<TMessage>, new()
{
    private static MessageParser<TMessage> s_parser = new(() => new TMessage());
    
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return s_parser.ParseFrom(data);
    }

    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return data.ToByteArray();
    }
}
