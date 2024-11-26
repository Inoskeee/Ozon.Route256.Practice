using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class ConsumeHandler<TKey, TValue> : IConsumeHandler<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>>
    where TKey : class
    where TValue : class
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;

    public ConsumeHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeHandler<TKey, TValue>> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task HandleAsync(
        KafkaMessage<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> message,
        CancellationToken cancellationToken)
    {
        if (message.Key.RawValue.Length == 0 && message.Value.RawValue.Length == 0)
        {
            _logger.LogWarning(
                "Key and value are empty, kafka message will not be processed. Topic={Topic}; Partition={Partition}; Offset={Offset};",
                message.TopicPartitionOffset.Topic,
                message.TopicPartitionOffset.Partition,
                message.TopicPartitionOffset.Offset);

            return;
        }

        await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();

        var processor = serviceScope.ServiceProvider.GetRequiredService<IMessageProcessor<TKey, TValue>>();

        await processor.ProcessMessageAsync(message.Key.Payload!, message.Value.Payload!, cancellationToken);
    }
}
