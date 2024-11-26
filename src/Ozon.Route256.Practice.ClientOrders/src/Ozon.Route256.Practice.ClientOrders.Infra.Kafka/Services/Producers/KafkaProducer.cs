using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services.Producers;

internal sealed class KafkaProducer : IKafkaProducer, IDisposable
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<long, string> _producer;
    
    public KafkaProducer(
        ILogger<KafkaProducer> logger,
        IOptions<KafkaSettings> kafkaSettings)
    {
        _logger = logger;
    
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
            Acks = Acks.Leader,
            EnableIdempotence = false,
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
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "KafkaProducer | Ошибка при отправке сообщения в топик {Topic}",
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