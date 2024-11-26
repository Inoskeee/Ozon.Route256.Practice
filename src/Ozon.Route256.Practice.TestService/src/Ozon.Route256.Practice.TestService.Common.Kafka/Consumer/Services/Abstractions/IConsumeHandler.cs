namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IConsumeHandler<TKey, TValue>
    where TKey : class
    where TValue : class
{
    Task HandleAsync(KafkaMessage<TKey, TValue> message, CancellationToken cancellationToken);
}
