namespace Ozon.Route256.OrderService.Kafka.Producers;

public interface IKafkaProducer
{
    Task SendMessage(
        string topic,
        string key,
        string value,
        CancellationToken token);

    Task SendProtoMessage(
        string topic,
        string key,
        byte[] value,
        CancellationToken token);
}