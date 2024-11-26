namespace Ozon.Route256.DataGenerator.Infra.Kafka;

public record KafkaTopics
{
    public required string OrdersInput { get; init; }
}
