namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IMessageProcessor<in TKey, in TValue>
    where TKey : class
    where TValue : class
{
    Task ProcessMessageAsync(TKey key, TValue payload, CancellationToken cancellationToken);
}
