namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public interface IKafkaConsumerFactory
{
    IKafkaConsumer CreateConsumer(ConsumerConfiguration configuration);
}
