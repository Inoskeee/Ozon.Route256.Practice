using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.TestService.Common.Data.Extensions;

namespace Ozon.Route256.TestService.Data.Customers;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomersData(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddPostgresDbFacade<CustomersDataConnectionOptions>(configuration.GetSection("Customers"))
            .AddScoped<ICustomersRepository, CustomersRepository>();
    }
}
