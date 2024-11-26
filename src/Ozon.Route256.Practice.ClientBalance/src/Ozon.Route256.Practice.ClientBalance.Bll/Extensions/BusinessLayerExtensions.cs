using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Services;
using Ozon.Route256.Practice.ClientBalance.Bll.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Services;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Extensions;

public static class BusinessLayerExtensions
{
    public static IServiceCollection ConfigureBllServices(this IServiceCollection services)
    {
        services.AddBllMappers();
        services.AddScoped<IClientBalanceService, ClientBalanceService>();
        return services;
    }
    
    private static IServiceCollection AddBllMappers(this IServiceCollection services)
    {
        services.AddScoped<IClientMapper, ClientMapper>();
        services.AddScoped<IChangeOperationMapper, ChangeOperationMapper>();
        services.AddScoped<IRegisterOperationMapper, RegisterOperationMapper>();
        services.AddScoped<IClientBalanceMapper, ClientBalanceMapper>();
        
        return services;
    }
}