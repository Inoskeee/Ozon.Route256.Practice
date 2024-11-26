using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Contracts.Kafka;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Services.Consumers;

internal sealed class KafkaConsumerProvider<TKey, TValue>
    (ILogger<KafkaConsumerProvider<TKey, TValue>> logger) 
    : IKafkaConsumerProvider<TKey, TValue>
{
    public IConsumer<TKey, TValue> Create(ConsumerConfig config, IDeserializer<TValue>? valueDeserializer = null)
    {
        var builder = new ConsumerBuilder<TKey, TValue>(config)
            .SetErrorHandler((_, error) => logger.LogError("Error reading topic: {error}", error))
            .SetLogHandler((_, message) => logger.LogInformation("Kafka consumed: {message}", message.Message));

        if (valueDeserializer is not null)
        {
            builder.SetValueDeserializer(valueDeserializer);
        }
        
        return builder.Build();
    }
}