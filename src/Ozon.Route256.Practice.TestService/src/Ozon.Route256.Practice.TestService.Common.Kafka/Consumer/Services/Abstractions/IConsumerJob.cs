namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IConsumerJob<TKey, TValue>
    where TKey : class
    where TValue : class
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}
