using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Services;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Services;

namespace Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Extensions;

public static class CustomerServiceExtensions
{
    private const string CustomerServiceUrl = "ROUTE256_CUSTOMER_SERVICE_GRPC_URL";
    
    public static IServiceCollection ConfigureCustomerServiceInfrastructure(this IServiceCollection services)
    {
        services.ConfigureCustomerServiceMappers();

        services.ConfigureCustomerClient();
        
        services.AddScoped<ICustomerGrpcClientProvider, CustomerGrpcClientProvider>();
        services.AddScoped<ICustomerServiceProvider, CustomerServiceProvider>();
        
        return services;
    }

    private static IServiceCollection ConfigureCustomerServiceMappers(this IServiceCollection services)
    {
        services.AddScoped<IRegionMapper, RegionMapper>();
        return services;
    }
    
    private static IServiceCollection ConfigureCustomerClient(this IServiceCollection services)
    {
        string? customerServiceUrl = Environment.GetEnvironmentVariable(CustomerServiceUrl);

        if (string.IsNullOrWhiteSpace(customerServiceUrl))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {CustomerServiceUrl}. Заполните ее и перезапустите приложение");
        }
        
        services.AddGrpcClient<Practice.CustomerService.CustomerService.CustomerServiceClient>(options =>
        {
            options.Address = new Uri(customerServiceUrl);
        });

        return services;
    }
}