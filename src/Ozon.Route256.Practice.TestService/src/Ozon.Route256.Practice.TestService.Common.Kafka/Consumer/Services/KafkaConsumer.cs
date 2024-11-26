using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly ILogger _logger;
    private readonly IConsumer<byte[], byte[]> _internalConsumer;

    public KafkaConsumer(IConsumer<byte[], byte[]> consumer, ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        _internalConsumer = consumer;
    }

    /// <inheritdoc cref="IKafkaConsumer.Consume"/>
    public ConsumeResult<byte[], byte[]>? Consume(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                return _internalConsumer.Consume(cancellationToken);
            }
            catch (ConsumeException ex) when (!ex.Error.IsFatal)
            {
                _logger.LogWarning(ex, "Non-fatal consume exception");
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
            {
                // stop consuming on token cancellation
                break;
            }
        }

        return null;
    }

    public void StoreOffset(ConsumeResult<byte[], byte[]> consumed)
    {
        _internalConsumer.StoreOffset(consumed);
    }

    public void Subscribe(string topic)
    {
        _internalConsumer.Subscribe(topic);
    }

    public void Dispose()
    {
        // consumer.Close() is needed to safely stop the consumer (commit offsets, leave group and so on) which is not done by .Dispose()
        _internalConsumer.Close();
        _internalConsumer.Dispose();
    }
}
