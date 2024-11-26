using Confluent.Kafka;

namespace Ozon.Route256.OrderService.Kafka.Settings;

public class ProducerSettings
{
    public Acks Acks { get; set; } = Acks.Leader;

    public bool EnableIdempotence { get; set; } = false;
}