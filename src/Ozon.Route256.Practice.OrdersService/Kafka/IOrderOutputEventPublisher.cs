using Ozon.Route256.OrderService.Proto.Messages;

namespace Ozon.Route256.OrderService.Kafka;

public interface IOrderOutputEventPublisher
{
    Task PublishToKafka(
        OrderOutputEventMessage message,
        CancellationToken token);
}