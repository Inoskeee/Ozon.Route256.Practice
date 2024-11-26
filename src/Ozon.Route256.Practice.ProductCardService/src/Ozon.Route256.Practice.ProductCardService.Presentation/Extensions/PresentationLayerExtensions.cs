using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Ozon.Route256.Practice.ProductCardService.Presentation.Contracts.Mappers;
using Ozon.Route256.Practice.ProductCardService.Presentation.Controllers.Grpc.Interceptors;
using Ozon.Route256.Practice.ProductCardService.Presentation.Mappers;

namespace Ozon.Route256.Practice.ProductCardService.Presentation.Extensions;

public static class PresentationLayerExtensions
{
    public static void ConfigurePresentationLayer(this IServiceCollection services)
    {
        services.AddPresentationMappers();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Ozon.Route256.Practice.ProductCardService", Version = "v1" });
            options.MapType<Google.Protobuf.WellKnownTypes.Timestamp>(schemaFactory: () => new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Example = new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            });
        });
        
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<ProductCardLoggerInterceptor>();
            options.Interceptors.Add<ProductCardExceptionInterceptor>();
        });
        
        services.AddGrpcReflection();
        services.AddGrpcSwagger();
    }
    
    private static void AddPresentationMappers(this IServiceCollection services)
    {
        services.AddScoped<IProductMapper, ProductMapper>();
    }
}