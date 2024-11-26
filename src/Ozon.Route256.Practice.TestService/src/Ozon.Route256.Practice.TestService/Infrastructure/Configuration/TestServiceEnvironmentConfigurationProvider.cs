namespace Ozon.Route256.TestService;

public class TestServiceEnvironmentConfigurationProvider : ConfigurationProvider
{
    private const string KafkaBrokers = "ROUTE256_KAFKA_BROKERS";
    private const string OrdersConnectionString = "ROUTE256_ORDER_SERVICE_DB_CONNECTION_STRING";
    private const string CustomersConnectionString = "ROUTE256_CUSTOMER_SERVICE_DB_CONNECTION_STRING";
    private const string TimeoutSeconds = "ROUTE256_TEST_SERVICE_TIMEOUT_SECONDS";

    public override void Load()
    {
        var kafkaBrokers = Environment.GetEnvironmentVariable(KafkaBrokers);
        var ordersConnectionString = Environment.GetEnvironmentVariable(OrdersConnectionString);
        var customersConnectionString = Environment.GetEnvironmentVariable(CustomersConnectionString);
        var samplingDuration = Environment.GetEnvironmentVariable(TimeoutSeconds);

        Data = new Dictionary<string, string?>
        {
            ["Integrations:OrdersOutputEvents:Kafka:BootstrapServers"] = kafkaBrokers,
            ["Integrations:OrdersInputErrors:Kafka:BootstrapServers"] = kafkaBrokers,
            ["Data:Orders:ConnectionString"] = ordersConnectionString,
            ["Data:Customers:ConnectionString"] = customersConnectionString,
            ["Mismatch:SamplingDurationSec"] = samplingDuration,
        };
    }
}
