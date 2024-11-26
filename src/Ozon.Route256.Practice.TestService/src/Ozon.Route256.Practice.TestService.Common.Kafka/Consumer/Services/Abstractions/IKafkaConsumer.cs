using Confluent.Kafka;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IKafkaConsumer : IDisposable
{
    ConsumeResult<byte[], byte[]>? Consume(CancellationToken cancellationToken);

    void StoreOffset(ConsumeResult<byte[], byte[]> consumed);

    void Subscribe(string topic);
}
