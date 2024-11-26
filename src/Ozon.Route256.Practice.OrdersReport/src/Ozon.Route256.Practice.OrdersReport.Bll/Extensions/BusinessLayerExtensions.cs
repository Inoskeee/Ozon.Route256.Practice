using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.OrdersReport.Bll.Configuration;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Helpers;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;
using Ozon.Route256.Practice.OrdersReport.Bll.Helpers;
using Ozon.Route256.Practice.OrdersReport.Bll.Mappers;
using Ozon.Route256.Practice.OrdersReport.Bll.Services;
using Ozon.Route256.Practice.OrdersReport.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Extensions;

public static class BusinessLayerExtensions
{
    public static IServiceCollection ConfigureBllServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGrpc(options => { options.EnableDetailedErrors = true; });

        services.ConfigureBllOptions(configuration);
        services.ConfigureMappers();

        services.ConfigureOrderGrpcClient();

        services.AddSingleton<IOrdersReportStateProvider, OrdersReportStateProvider>();

        services.AddScoped<IOrderGrpcClientProvider, OrderGrpcClientProvider>();
        services.AddScoped<IOrdersReportService, OrdersReportService>();
        return services;
    }

    private static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IReportMapper, ReportMapper>();
        return services;
    }

    private static IServiceCollection ConfigureBllOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RateLimitConfig>(configuration.GetSection(nameof(RateLimitConfig)));
        services.Configure<GrpcClientPackageConfig>(configuration.GetSection(nameof(GrpcClientPackageConfig)));
        return services;
    }
}