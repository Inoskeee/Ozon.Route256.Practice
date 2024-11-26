using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ozon.Route256.Practice.ClientOrders.Presentation.Controllers.Grpc.Interceptors;
using Ozon.Route256.Practice.ClientOrders.Presentation.Mappers;
using Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Extensions;

public static class PresentationLayerExtensions
{
    public static IServiceCollection ConfigurePresentationServices(this IServiceCollection services)
    {
        services.AddPresentationMappers();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Ozon.Route256.Practice.ClientOrders", Version = "v1" });
        });
        
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<ClientOrdersLoggerInterceptor>();
            options.Interceptors.Add<ClientOrdersExceptionInterceptor>();
        });
        
        services.AddGrpcReflection();
        services.AddGrpcSwagger();
        
        return services;
    }
    
    private static IServiceCollection AddPresentationMappers(this IServiceCollection services)
    {
        services.AddScoped<IItemsMapper, ItemsMapper>();
        services.AddScoped<IOrdersMapper, OrdersMapper>();
        
        return services;
    }
}