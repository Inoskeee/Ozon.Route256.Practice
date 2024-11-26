using Google.Protobuf;
using Ozon.Route256.OrderService.Kafka.Producers;
using Ozon.Route256.OrderService.Proto.Messages;

namespace Ozon.Route256.OrderService.Kafka;

public class OrderOutputEventPublisher : IOrderOutputEventPublisher
{
    private readonly IKafkaProducer _kafkaProducer;
    private const string Topic = "order_output_events";

    public OrderOutputEventPublisher(
        IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task PublishToKafka(
        OrderOutputEventMessage message,
        CancellationToken token)
    {
        await _kafkaProducer.SendProtoMessage(
            Topic,
            key: message.OrderId.ToString(),
            value: message.ToByteArray(),
            token);
    }
}