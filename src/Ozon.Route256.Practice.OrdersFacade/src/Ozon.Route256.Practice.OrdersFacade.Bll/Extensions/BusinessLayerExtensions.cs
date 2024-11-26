using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ozon.Route256.Practice.OrdersFacade.Bll.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Metrics;
using Ozon.Route256.Practice.OrdersFacade.Bll.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Metrics;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Extensions;

public static class BusinessLayerExtensions
{
    private const string JaegerConnection = "ROUTE256_JAEGER_URL";
    public static void ConfigureBllServices(this IServiceCollection services)
    {
        services.AddMetrics();
        services.AddScoped<IEntityMapper, EntityMapper>();
        
        services.AddScoped<IAggregateByCustomerBuilder, AggregateByCustomerBuilder>();
        services.AddScoped<IAggregateByRegionBuilder, AggregateByRegionBuilder>();
        
    }

    private static void AddMetrics(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("Orders-Facade-Service"));
                
                metrics.AddMeter(OrdersFacadeMeter.MeterName);
                metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                metrics.AddProcessInstrumentation();
                metrics.AddRuntimeInstrumentation();
                metrics.AddAspNetCoreInstrumentation();

                metrics.AddPrometheusExporter();
            })
            .WithTracing(trace =>
            {
                trace.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("Orders-Facade-Service")
                    .AddAttributes(new Dictionary<string, object> { { "from", "docker" } }));
                
                trace.AddAspNetCoreInstrumentation();
                trace.AddHttpClientInstrumentation();
                trace.SetSampler(new AlwaysOnSampler());
                trace.AddOtlpExporter(o => o.Endpoint = GetJaegerConnectionString());
            });
        
        services.AddSingleton<IOrdersFacadeMeter, OrdersFacadeMeter>();
    }

    private static Uri GetJaegerConnectionString()
    {
        string? jaegerConnnectionString = Environment.GetEnvironmentVariable(JaegerConnection);

        if (string.IsNullOrWhiteSpace(jaegerConnnectionString))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {JaegerConnection}. Заполните ее и перезапустите приложение");
        }

        return new Uri(jaegerConnnectionString);
    }
}