using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ClientOrders.Bll.Configuration;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Kafka;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Entities;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Serializers;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services.Consumers;

internal sealed class OrderInputErrorsConsumer(
    ILogger<OrderInputErrorsConsumer> logger,
    IKafkaConsumerProvider<Ignore, OrderInputErrorsMessage> consumerProvider,
    IOptions<KafkaSettings> kafkaSettings,
    IOptions<KafkaTopics> kafkaTopics)
    : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = new()
    {
        GroupId = "client_orders",
        BootstrapServers = kafkaSettings.Value.BootstrapServers,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false
    };


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OrderInputErrorsConsumer | Запуск");

        await Task.Yield();

        var consumer = consumerProvider.Create(_consumerConfig, new KafkaJsonSerializer<OrderInputErrorsMessage>());
        
        consumer.Subscribe(kafkaTopics.Value.OrdersInputErrors);
        
        logger.LogInformation("OrderInputErrorsConsumer | Создана подписка на топик {topic}", 
            kafkaTopics.Value.OrdersOutputEvents);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(timeout: TimeSpan.FromMilliseconds(100));

                if (message is null)
                {
                    continue;
                }
                
                logger.LogInformation("OrderInputErrorsConsumer | Получено сообщение {message}", 
                    JsonSerializer.Serialize(message.Message.Value));
                
                consumer.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex, 
                    "OrderInputErrorsConsumer | Ошибка во время получения сообщения: {exception}", 
                    ex.ToString());
            }
        }
        
        consumer.Unsubscribe();
        
        logger.LogInformation("OrderInputErrorsConsumer | Отписка от топика {topic}",
            kafkaTopics.Value.OrdersOutputEvents);
        logger.LogInformation("OrderInputErrorsConsumer | Завершено");
    }
}