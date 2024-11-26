using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Ozon.Route256.DataGenerator.Infra.Kafka.Interfaces;
using Ozon.Route256.DataGenerator.Infra.Kafka.Settings;

namespace Ozon.Route256.DataGenerator.Infra.Kafka;

public class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<long, string> _producer;

    public KafkaProducer(
        ILogger<KafkaProducer> logger,
        KafkaSettings kafkaSettings,
        ProducerSettings producerSettings)
    {
        _logger = logger;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            Acks = producerSettings.Acks,
            EnableIdempotence = producerSettings.EnableIdempotence,
            LingerMs = producerSettings.LingerMs
        };

        _producer = new ProducerBuilder<long, string>(producerConfig).Build();
    }

    public async Task SendMessage(
        long key,
        string value,
        string topic,
        CancellationToken token)
    {
        try
        {
            var message = new Message<long, string>
            {
                Key = key,
                Value = value
            };

            await _producer.ProduceAsync(topic, message, token);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Ошибка при отправке сообщения в топик {Topic}",
                topic);

            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _producer.Flush();
        _producer.Dispose();
    }
}
