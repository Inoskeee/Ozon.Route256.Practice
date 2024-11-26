using System.Text.Json;
using Microsoft.Extensions.Options;
using Ozon.Route256.DataGenerator.Bll.ProviderContracts;
using Ozon.Route256.DataGenerator.Infra.Exceptions;
using Ozon.Route256.DataGenerator.Infra.Kafka;
using Ozon.Route256.DataGenerator.Infra.Kafka.Interfaces;
using Ozon.Route256.DataGenerator.Messages;

namespace Ozon.Route256.DataGenerator.Infra.Providers;

public class KafkaProvider : IKafkaProvider
{
    private readonly IKafkaProducer _producer;
    private readonly string _ordersInputTopic;

    public KafkaProvider(
        IKafkaProducer producer,
        IOptions<KafkaTopics> kafkaTopics)
    {
        _producer = producer;

        _ordersInputTopic = string.IsNullOrWhiteSpace(kafkaTopics.Value.OrdersInput)
            ? throw new InvalidKafkaConfigurationException($"Topic name {nameof(kafkaTopics.Value.OrdersInput)} is empty")
            : kafkaTopics.Value.OrdersInput;
    }

    public Task Publish(OrderInputMessage message, CancellationToken token)
    {
        var key = message.CustomerId;
        var value = JsonSerializer.Serialize(message);

        return _producer.SendMessage(
            key,
            value,
            _ordersInputTopic,
            token);
    }
}
