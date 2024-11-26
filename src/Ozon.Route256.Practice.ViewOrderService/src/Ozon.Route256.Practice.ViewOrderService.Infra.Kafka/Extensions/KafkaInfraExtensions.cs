using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Contracts.Kafka;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Services.Consumers;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Extensions;

public static class KafkaInfraExtensions
{
    private const string KafkaConnectionString = "ROUTE256_KAFKA_BROKERS";
    
    public static void ConfigureKafkaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureKafkaOptions(configuration);

        services.AddHostedService<OrderOutputEventConsumer>();
        
        services.AddSingleton<IKafkaConsumerProvider<Ignore, OrderOutputEventMessage>, KafkaConsumerProvider<Ignore, OrderOutputEventMessage>>();
    }
    
    private static void ConfigureKafkaOptions(this IServiceCollection services, IConfiguration configuration)
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
    }
    
}