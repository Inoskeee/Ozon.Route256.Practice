using System.Reflection;
using FluentMigrator.Runner;
using Ozon.Route256.OrderService.Bll.Helpers;
using Ozon.Route256.OrderService.Bll.Helpers.Interfaces;
using Ozon.Route256.OrderService.Bll.Services;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Dal.Interfaces;
using Ozon.Route256.OrderService.Dal.Repositories;
using Ozon.Route256.OrderService.Kafka;
using Ozon.Route256.OrderService.Kafka.Consumers;
using Ozon.Route256.OrderService.Kafka.Producers;
using Ozon.Route256.OrderService.Kafka.Settings;

namespace Ozon.Route256.OrderService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("ORDER_SERVICE_DB_CONNECTION_STRING");

        services.AddMigration(connectionString);

        return services;
    }
    
    private static IServiceCollection AddMigration(this IServiceCollection services,
        string connectionString)
    {
        return services.AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                x => x.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly())
                    .For.Migrations());
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, Bll.Services.OrderService>();
        services.AddScoped<ILogsService, LogsService>();
        services.AddScoped<IInputValidationHelper, InputValidationHelper>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<ILogsRepository, LogsRepository>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        
        return services;
    }

    public static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var kafkaSettings = configuration.GetSection("Kafka").Get<KafkaSettings>();
        var bootstrapServers = configuration.GetValue<string>("KAFKA_BROKERS");

        kafkaSettings.BootstrapServers = bootstrapServers;
        var consumerSettings = configuration.GetSection("Kafka:Consumer:OrdersInputConsumer").Get<ConsumerSettings>();

        services.AddHostedService(
            serviceProvider => new InputOrderBackgroundConsumer(serviceProvider, kafkaSettings!, consumerSettings!));

        services.AddProducer(configuration, kafkaSettings);
        services.AddTransient<IOrderInputErrorsPublisher, OrderInputErrorsPublisher>();
        services.AddTransient<IOrderOutputEventPublisher, OrderOutputEventPublisher>();

        return services;
    }

    private static IServiceCollection AddProducer(
        this IServiceCollection services,
        IConfiguration configuration,
        KafkaSettings kafkaSettings)
    {
        var producerSettings = configuration.GetSection("Kafka:Producer").Get<ProducerSettings>();

        services.AddSingleton<IKafkaProducer, KafkaProducer>(
            serviceProvider => new KafkaProducer(
                serviceProvider.GetRequiredService<ILogger<KafkaProducer>>(),
                kafkaSettings,
                producerSettings!));

        return services;
    }
}