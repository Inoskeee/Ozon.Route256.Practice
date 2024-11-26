using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaConsumer<TKey, TValue, TProcessor>(
        this IServiceCollection services,
        IConfiguration config,
        Action<IKafkaConsumerBuilder<TKey, TValue>> configureBuilder)
        where TKey : class
        where TValue : class
        where TProcessor : class, IMessageProcessor<TKey, TValue>
    {
        var processorName = typeof(TProcessor).Name;

        services.TryAddSingleton<IKafkaConsumerFactory, KafkaConsumerFactory>();

        services.Configure<ConsumerConfiguration>(processorName, config);

        configureBuilder(new KafkaConsumerBuilder<TKey, TValue>(services));

        services.AddScoped<IMessageProcessor<TKey, TValue>, TProcessor>();

        services.AddSingleton<IMessageDeserializer<TKey, TValue>, MessageDeserializer<TKey, TValue>>();

        services.AddSingleton<IConsumeHandler<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>>, ConsumeHandler<TKey, TValue>>();

        services.AddSingleton<IConsumerJob<TKey, TValue>>(provider =>
        {
            var kafkaConsumerFactory = provider.GetRequiredService<IKafkaConsumerFactory>();
            var messageDeserializer = provider.GetRequiredService<IMessageDeserializer<TKey, TValue>>();
            var handler = provider.GetRequiredService<IConsumeHandler<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>>>();
            var consumerConfig = provider.GetRequiredService<IOptionsMonitor<ConsumerConfiguration>>();

            return new SingleConsumerJob<TKey, TValue>(
                kafkaConsumerFactory,
                messageDeserializer,
                handler,
                consumerConfig.Get(processorName));
        });

        return services.AddHostedService<ConsumerBackgroundService<TKey, TValue>>();
    }
}
