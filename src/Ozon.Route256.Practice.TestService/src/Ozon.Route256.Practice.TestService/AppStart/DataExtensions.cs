using System.Diagnostics.CodeAnalysis;
using Ozon.Route256.TestService.Data;
using Ozon.Route256.TestService.Data.Customers;
using Ozon.Route256.TestService.Data.Orders;

namespace Ozon.Route256.TestService.AppStart;

[ExcludeFromCodeCoverage]
public static class DataExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMismatchDataServices(configuration)
            .AddCustomersData(configuration)
            .AddOrdersData(configuration);
    }
}
