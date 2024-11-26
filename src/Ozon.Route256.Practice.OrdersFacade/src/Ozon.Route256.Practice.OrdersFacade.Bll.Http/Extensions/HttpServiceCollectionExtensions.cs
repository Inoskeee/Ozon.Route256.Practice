using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Configuration;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Extensions;

public static class HttpServiceCollectionExtensions
{
    private const string OrderUrls = "ROUTE256_ORDER_SERVICE_HTTP_URLS";
    private const string CustomerUrl = "ROUTE256_CUSTOMER_SERVICE_HTTP_URL";
    
    public static IServiceCollection ConfigureHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMappers();

        services.ConfigureCustomerClient(configuration);
        services.ConfigureOrderClient(configuration);
        
        services.AddScoped<ICustomerHttpClientProvider, CustomerHttpClientProvider>();
        services.AddScoped<IOrderHttpClientProvider, OrderHttpClientProvider>();
        services.AddScoped<IOrderFacadeHttpService, OrderFacadeHttpService>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IHttpCustomerMapper, HttpCustomerMapper>();
        services.AddScoped<IHttpOrderMapper, HttpOrderMapper>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureOrderClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OrderServiceConfiguration>(options =>
        {
            configuration.GetSection(nameof(OrderServiceConfiguration)).Bind(options);

            var orderServiceUrls = Environment.GetEnvironmentVariable(OrderUrls);
            if (!string.IsNullOrEmpty(orderServiceUrls))
            {
                options.Urls = orderServiceUrls
                    .Split(',')
                    .Select(url => url.Trim())
                    .ToArray();
            }
            else
            {
                throw new InvalidOperationException(
                    $"Отсутствует переменная окружения {OrderUrls}. Заполните ее и перезапустите приложение");
            }
        });
        
        return services;
    }
    
    private static IServiceCollection ConfigureCustomerClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CustomerServiceConfiguration>(options =>
        {
            configuration.GetSection(nameof(CustomerServiceConfiguration)).Bind(options);

            var customerServiceUrl = Environment.GetEnvironmentVariable(CustomerUrl);
            if (!string.IsNullOrEmpty(customerServiceUrl))
            {
                options.Url = customerServiceUrl.Trim();
            }
            else
            {
                throw new InvalidOperationException(
                    $"Отсутствует переменная окружения {CustomerUrl}. Заполните ее и перезапустите приложение");
            }
        });
        
        return services;
    }
}