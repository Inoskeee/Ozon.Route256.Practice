using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ClientOrders.Bll.Configuration;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Services;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Extensions;

public static class BusinessLayerExtensions
{
    public static IServiceCollection ConfigureBllServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.ConfigureBllOptions(configuration);
        
        services.AddScoped<IClientOrdersService, ClientOrdersService>();
        return services;
    }
    
    private static IServiceCollection ConfigureBllOptions(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddOptions<ClientOrdersSettings>()
            .Configure(configuration.GetSection(nameof(ClientOrdersSettings)).Bind);
        
        return services;
    }
}