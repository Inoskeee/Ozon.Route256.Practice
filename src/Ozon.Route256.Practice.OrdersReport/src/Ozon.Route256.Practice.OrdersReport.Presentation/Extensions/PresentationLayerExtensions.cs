using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Extensions;

public static class PresentationLayerExtensions
{
    public static IServiceCollection ConfigurePresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "API Ozon.Route256.Practice.OrdersReport", Version = "v1" });
        });

        return services;
    }
}