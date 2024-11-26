using Confluent.Kafka;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Kafka;

internal interface IKafkaConsumerProvider<TKey, TValue>
{
    IConsumer<TKey, TValue> Create(ConsumerConfig config, IDeserializer<TValue>? valueDeserializer = null);
}