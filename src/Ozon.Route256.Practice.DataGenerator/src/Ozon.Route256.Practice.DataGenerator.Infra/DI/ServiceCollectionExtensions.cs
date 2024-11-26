using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ozon.Route256.DataGenerator.Bll.ProviderContracts;
using Ozon.Route256.DataGenerator.Infra.Kafka;
using Ozon.Route256.DataGenerator.Infra.Kafka.Interfaces;
using Ozon.Route256.DataGenerator.Infra.Kafka.Settings;
using Ozon.Route256.DataGenerator.Infra.Providers;
using Ozon.Route256.DataGenerator.Infra.Workers;
using CustomerServiceClient = Ozon.Route256.CustomerService.CustomerService.CustomerServiceClient;

namespace Ozon.Route256.DataGenerator.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddGrpcClients(configuration)
            .AddKafka(configuration)
            .AddProviders()
            .AddJobs(configuration);
    }

    private static IServiceCollection AddGrpcClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddCustomerServiceGrpcClient(configuration);
    }

    private static IServiceCollection AddCustomerServiceGrpcClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var customerServiceUrl = configuration.GetValue<string>("CUSTOMER_SERVICE_URL");

        services.AddGrpcClient<CustomerServiceClient>(options =>
            {
                options.Address = new Uri(customerServiceUrl!);
            })
            .ConfigureChannel(options =>
            {
                options.ServiceConfig = new ServiceConfig
                {
                    MethodConfigs = { GetDefaultMethodConfig() }
                };
            });

        return services;
    }

    private static MethodConfig GetDefaultMethodConfig()
    {
        return new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromMilliseconds(10),
                MaxBackoff = TimeSpan.FromMilliseconds(25),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };
    }

    private static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KafkaTopics>(configuration.GetSection(nameof(KafkaTopics)));

        return services.AddProducer(configuration);
    }

    private static IServiceCollection AddProducer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var kafkaSettings = new KafkaSettings
        {
            BootstrapServers = configuration.GetValue<string>("KAFKA_BROKERS")
        };

        var producerSettings = configuration
            .GetSection(key: "ProducerSettings")
            .Get<ProducerSettings>();

        services.AddSingleton<IKafkaProducer, KafkaProducer>(sp => new KafkaProducer(
            sp.GetRequiredService<ILogger<KafkaProducer>>(),
            kafkaSettings,
            producerSettings!));

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        return services
            .AddScoped<ICustomerProvider, CustomerProvider>()
            .AddScoped<IKafkaProvider, KafkaProvider>();
    }

    private static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrdersGeneratorSettings>(settings =>
        {
            settings.OrdersPerSecond = configuration.GetValue<int>("ORDERS_PER_SECOND");
            settings.CustomersPerSecond = configuration.GetValue<int>("CUSTOMERS_PER_SECOND");
            settings.InvalidOrderCounterNumber = configuration.GetValue<int>("INVALID_ORDER_COUNTER_NUMBER");
        });

        return services.AddHostedService<OrdersGeneratorWorker>();
    }
}
