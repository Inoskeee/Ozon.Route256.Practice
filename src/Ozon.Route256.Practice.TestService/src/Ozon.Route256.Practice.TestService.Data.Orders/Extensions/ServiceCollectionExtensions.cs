using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.TestService.Common.Data.Extensions;

namespace Ozon.Route256.TestService.Data.Orders;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrdersData(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPostgresDbFacade<OrdersDataConnectionOptions>(configuration.GetSection("Orders"))
            .AddScoped<IOrdersRepository, OrdersRepository>();
    }
}
