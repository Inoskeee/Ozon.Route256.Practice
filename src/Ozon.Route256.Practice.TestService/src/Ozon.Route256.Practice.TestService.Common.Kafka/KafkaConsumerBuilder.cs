using Microsoft.Extensions.DependencyInjection;

namespace Ozon.Route256.TestService.Common.Kafka;

public class KafkaConsumerBuilder<TKey, TValue> : IKafkaConsumerBuilder<TKey, TValue>
    where TKey : class
    where TValue : class
{
    public KafkaConsumerBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}
