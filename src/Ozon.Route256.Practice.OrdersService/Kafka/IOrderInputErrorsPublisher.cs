using Ozon.Route256.OrderService.Kafka.Messages;

namespace Ozon.Route256.OrderService.Kafka;

public interface IOrderInputErrorsPublisher
{
    Task PublishToKafka(
        OrderInputErrorsMessage message,
        CancellationToken token);
}