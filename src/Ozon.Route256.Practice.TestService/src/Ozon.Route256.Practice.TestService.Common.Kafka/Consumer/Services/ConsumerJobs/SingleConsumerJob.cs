using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class SingleConsumerJob<TKey, TValue> : IConsumerJob<TKey, TValue>
    where TKey : class
    where TValue : class
{
    private readonly IKafkaConsumerFactory _kafkaConsumerFactory;
    private readonly IMessageDeserializer<TKey, TValue> _messageDeserializer;
    private readonly IConsumeHandler<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> _consumeHandler;
    private readonly ConsumerConfiguration _consumerConfig;

    public SingleConsumerJob(
        IKafkaConsumerFactory kafkaConsumerFactory,
        IMessageDeserializer<TKey, TValue> messageDeserializer,
        IConsumeHandler<KafkaMessagePart<TKey>, KafkaMessagePart<TValue>> consumeHandler,
        ConsumerConfiguration consumerConfig)
    {
        _kafkaConsumerFactory = kafkaConsumerFactory;
        _messageDeserializer = messageDeserializer;
        _consumeHandler = consumeHandler;
        _consumerConfig = consumerConfig;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        using var consumer = _kafkaConsumerFactory.CreateConsumer(_consumerConfig);

        consumer.Subscribe(_consumerConfig.Topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<byte[], byte[]>? consumed = consumer.Consume(cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                break;

            // ConsumeSingle returns null when consuming is cancelled. That's done to unify consuming process for single and batch consumers
            if (consumed == null)
                continue;

            var message = _messageDeserializer.DeserializeMessage(consumed);

            await _consumeHandler.HandleAsync(message, cancellationToken);

            consumer.StoreOffset(consumed);
        }
    }
}
