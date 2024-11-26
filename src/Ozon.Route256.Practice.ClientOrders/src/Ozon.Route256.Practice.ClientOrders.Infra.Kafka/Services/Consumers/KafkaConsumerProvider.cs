using Confluent.Kafka;
using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Kafka;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services.Consumers;

internal sealed class KafkaConsumerProvider<TKey, TValue>
    (ILogger<KafkaConsumerProvider<TKey, TValue>> logger) 
    : IKafkaConsumerProvider<TKey, TValue>
{
    public IConsumer<TKey, TValue> Create(ConsumerConfig config, IDeserializer<TValue>? valueDeserializer = null)
    {
        var builder = new ConsumerBuilder<TKey, TValue>(config)
            .SetErrorHandler((_, error) => logger.LogError($"Error reading topic: {error}"))
            .SetLogHandler((_, message) => logger.LogInformation($"Kafka consumed: {message.Message}"));

        if (valueDeserializer is not null)
        {
            builder.SetValueDeserializer(valueDeserializer);
        }
        
        return builder.Build();
    }
}