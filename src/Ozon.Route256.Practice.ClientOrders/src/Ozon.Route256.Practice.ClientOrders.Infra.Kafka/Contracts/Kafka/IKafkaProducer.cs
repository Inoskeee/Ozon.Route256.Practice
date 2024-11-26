namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts;

internal interface IKafkaProducer
{
    Task SendMessage(
        long key,
        string value,
        string topic,
        CancellationToken token);
}