using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Contracts.Kafka;
using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Serializers;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Services.Consumers;

internal sealed class OrderOutputEventConsumer(
    ILogger<OrderOutputEventConsumer> logger,
    IKafkaConsumerProvider<Ignore, OrderOutputEventMessage> consumerProvider,
    IOptions<KafkaSettings> kafkaSettings,
    IOptions<KafkaTopics> kafkaTopics,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = new()
    {
        GroupId = "view_order_service",
        BootstrapServers = kafkaSettings.Value.BootstrapServers,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false
    };

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("OrderOutputEventConsumer | Запуск");

        await Task.Yield();

        var consumer = consumerProvider.Create(_consumerConfig, new KafkaProtoSerializer<OrderOutputEventMessage>());
        
        consumer.Subscribe(kafkaTopics.Value.OrdersOutputEvents);
        
        logger.LogInformation("OrderOutputEventConsumer | Создана подписка на топик {topic}", 
            kafkaTopics.Value.OrdersOutputEvents);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = consumer.Consume(timeout: TimeSpan.FromMilliseconds(100));

                if (message is null)
                {
                    continue;
                }
                
                logger.LogInformation("OrderOutputEventConsumer | Получено сообщение {message}", 
                    JsonSerializer.Serialize(message.Message.Value));

                if (message.Message.Value.EventType is OutputEventType.Created or OutputEventType.Updated)
                {
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var ordersServiceProvider = scope.ServiceProvider.GetRequiredService<IOrdersServiceProvider>();
                        var ordersResult = await ordersServiceProvider.GetOrder(
                            message.Message.Value.OrderId);
                        
                        if (ordersResult is not null)
                        {
                            var postgresProvider = scope.ServiceProvider.GetRequiredService<IPostgresProvider>();
                            await postgresProvider.UpdateOrInsertOrder(ordersResult, cancellationToken);
                        }
                    }
                }
                
                consumer.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex, 
                    "OrderOutputEventConsumer | Ошибка во время получения сообщения: {exception}", 
                    ex.ToString());
            }
        }
        
        consumer.Unsubscribe();
        
        logger.LogInformation("OrderOutputEventConsumer | Отписка от топика {topic}",
            kafkaTopics.Value.OrdersOutputEvents);
        logger.LogInformation("OrderOutputEventConsumer | Завершено");
    }
}