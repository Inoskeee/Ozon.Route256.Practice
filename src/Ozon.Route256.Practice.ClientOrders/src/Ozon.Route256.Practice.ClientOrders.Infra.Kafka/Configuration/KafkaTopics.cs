namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Configuration;

public class KafkaTopics
{
    public string OrdersInput { get; set; } = null!;
    
    public string OrdersOutputEvents { get; set; } = null!;
    
    public string OrdersInputErrors { get; set; } = null!;
}