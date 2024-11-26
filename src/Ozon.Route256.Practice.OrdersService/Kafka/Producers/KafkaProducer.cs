using Confluent.Kafka;
using Ozon.Route256.OrderService.Kafka.Settings;

namespace Ozon.Route256.OrderService.Kafka.Producers;

public class KafkaProducer : IDisposable,
    IKafkaProducer
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly IProducer<string, byte[]> _protoProducer;

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
            EnableIdempotence = producerSettings.EnableIdempotence
        };
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        _protoProducer = new ProducerBuilder<string, byte[]>(producerConfig).Build();
    }

    public async Task SendMessage(
        string topic,
        string key,
        string value,
        CancellationToken token)
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = key,
                Value = value
            };

            await _producer.ProduceAsync(topic, message, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in producing to {Topic}", topic);
            throw;
        }
    }

    public async Task SendProtoMessage(
        string topic,
        string key,
        byte[] value,
        CancellationToken token)
    {
        try
        {
            var message = new Message<string, byte[]>
            {
                Key = key,
                Value = value
            };

            await _protoProducer.ProduceAsync(topic, message, token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in producing to {Topic}", topic);
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