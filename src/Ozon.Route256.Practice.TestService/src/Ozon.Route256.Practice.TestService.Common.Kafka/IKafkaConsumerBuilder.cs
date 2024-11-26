using Microsoft.Extensions.DependencyInjection;

namespace Ozon.Route256.TestService.Common.Kafka;

public interface IKafkaConsumerBuilder<TKey, TValue>
    where TValue : class
{
    IServiceCollection Services { get; }
}
