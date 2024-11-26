using System.Text.Json;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Configuration;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Services;

internal sealed class KafkaProvider : IKafkaProvider
{
    private readonly IKafkaProducer _producer;
    private readonly string _ordersInputTopic;

    public KafkaProvider(
        IKafkaProducer producer,
        IOptions<KafkaTopics> kafkaTopics)
    {
        _producer = producer;
        _ordersInputTopic = kafkaTopics.Value.OrdersInput;
    }
    
    public async Task<bool> Publish(OrderInputModel message, CancellationToken token)
    {
        var key = message.CustomerId;
        var value = JsonSerializer.Serialize(message);

        var result = _producer.SendMessage(
            key,
            value,
            _ordersInputTopic,
            token);
        
        return true;
    }
}