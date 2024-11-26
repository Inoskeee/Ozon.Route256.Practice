using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Ozon.Route256.Practice.ClientBalance.Presentation.Controllers.Grpc.Interceptors;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Extensions;

public static class PresentationLayerExtensions
{
    public static IServiceCollection ConfigurePresentationServices(this IServiceCollection services)
    {
        services.AddPresentationMappers();
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Ozon.Route256.Practice.ClientBalance", Version = "v1" });
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
            options.Interceptors.Add<ClientBalanceLoggerInterceptor>();
            options.Interceptors.Add<ClientBalanceExceptionInterceptor>();
        });
        
        services.AddGrpcReflection();
        services.AddGrpcSwagger();
        
        return services;
    }
    
    private static IServiceCollection AddPresentationMappers(this IServiceCollection services)
    {
        services.AddScoped<IClientRequestMapper, ClientRequestMapper>();
        services.AddScoped<IChangeOperationRequestMapper, ChangeOperationRequestMapper>();
        services.AddScoped<IRegisterOperationRequestMapper, RegisterOperationRequestMapper>();

        return services;
    }
}