using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Kafka;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services.Consumers;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services.Producers;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Extensions;

public static class KafkaInfraExtensions
{
    private const string KafkaConnectionString = "ROUTE256_KAFKA_BROKERS";
    
    public static IServiceCollection ConfigureKafkaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureKafkaMappers();
        services.ConfigureKafkaOptions(configuration);

        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        services.AddScoped<IKafkaProvider, KafkaProvider>();

        services.AddHostedService<OrderOutputEventConsumer>();
        
        services.AddSingleton<IKafkaConsumerProvider<Ignore, OrderOutputEventMessage>, KafkaConsumerProvider<Ignore, OrderOutputEventMessage>>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureKafkaMappers(this IServiceCollection services)
    {
        services.AddScoped<IOrderInputMapper, OrderInputMapper>();
        return services;
    }
    
    private static IServiceCollection ConfigureKafkaOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KafkaTopics>()
            .Configure(configuration.GetSection(nameof(KafkaTopics)).Bind);

        services.AddOptions<KafkaSettings>()
            .Configure(options =>
            {
                var kafkaConnectionString = Environment.GetEnvironmentVariable(KafkaConnectionString);

                if (string.IsNullOrEmpty(kafkaConnectionString))
                {
                    throw new InvalidOperationException(
                        $"Отсутствует переменная окружения {KafkaConnectionString}. Заполните ее и перезапустите приложение");
                }
                
                options.BootstrapServers = kafkaConnectionString;
            });
        
        return services;
    }
    
}