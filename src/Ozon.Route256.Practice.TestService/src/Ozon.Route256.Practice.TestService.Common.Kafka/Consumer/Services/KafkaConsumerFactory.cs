using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public class KafkaConsumerFactory : IKafkaConsumerFactory
{
    private readonly ILogger<KafkaConsumerFactory> _logger;
    private readonly ILogger<KafkaConsumer> _consumerLogger;

    public KafkaConsumerFactory(ILogger<KafkaConsumerFactory> logger, ILogger<KafkaConsumer> consumerLogger)
    {
        _logger = logger;
        _consumerLogger = consumerLogger;
    }

    public IKafkaConsumer CreateConsumer(ConsumerConfiguration configuration)
    {
        _logger.LogInformation(
            "Create new consumer. Brokers: {Brokers}, GroupId: {GroupId}, Topic: {Topic}",
            configuration.BootstrapServers,
            configuration.GroupId,
            configuration.Topic);

        var consumer = new ConsumerBuilder<byte[], byte[]>(configuration).Build();

        return new KafkaConsumer(consumer, _consumerLogger);
    }
}
