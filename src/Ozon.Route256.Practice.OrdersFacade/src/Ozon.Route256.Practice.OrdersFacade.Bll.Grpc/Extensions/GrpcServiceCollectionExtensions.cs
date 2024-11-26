using FluentValidation;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Interceptors;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Validators;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Extensions;

public static class GrpcServiceCollectionExtensions
{
    private const string OrderUrls = "ROUTE256_ORDER_SERVICE_GRPC_URLS";
    private const string CustomerUrl = "ROUTE256_CUSTOMER_SERVICE_GRPC_URL";
    
    public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<OrderFacadeLoggerInterceptor>();
            options.Interceptors.Add<OrderFacadeExceptionInterceptor>();
        });
        
        services.AddGrpcReflection();
        
        services.ConfigureOrderClient();
        services.ConfigureCustomerClient();

        services.RegisterMapsterGrpcConfiguration();
        services.ConfigureMappers();
        
        services
            .AddScoped<IResponseValidatorProvider<GetOrderByCustomerRequest, GetOrderByCustomerResponse>,
                OrderByCustomerResponseValidator>();
        
        services.AddScoped<ICustomerGrpcClientProvider, CustomerGrpcClientProvider>();
        services.AddScoped<IOrderGrpcClientProvider, OrderGrpcClientProvider>();

        services.AddValidatorsFromAssemblyContaining<QueryOrdersByCustomerValidator>();
        services.AddValidatorsFromAssemblyContaining<QueryOrdersByRegionValidator>();
        
        return services;
    }

    private static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IGrpcCustomerMapper, GrpcCustomerMapper>();
        services.AddScoped<IGrpcOrderMapper, GrpcOrderMapper>();
        services.AddScoped<IGrpcAggregateResponseMapper, GrpcAggregateResponseMapper>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureOrderClient(this IServiceCollection services)
    {
        StaticResolverFactory orderFactory = new StaticResolverFactory(orderSerivce => 
            ConfigureOrderBalancerAddresses());

        services.AddSingleton<ResolverFactory>(_ => orderFactory);
        
        services.AddGrpcClient<OrderGrpc.OrderGrpc.OrderGrpcClient>(options =>
        {
            options.Address = new Uri("static://orderSerivce");
            
        }).ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
            x.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
        });

        return services;
    }

    private static IServiceCollection ConfigureCustomerClient(this IServiceCollection services)
    {
        string? customerServiceUrl = Environment.GetEnvironmentVariable(CustomerUrl);

        if (string.IsNullOrWhiteSpace(customerServiceUrl))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {CustomerUrl}. Заполните ее и перезапустите приложение");
        }
        
        services.AddGrpcClient<CustomerService.CustomerService.CustomerServiceClient>(options =>
        {
            options.Address = new Uri(customerServiceUrl);
        });

        return services;
    }
    
    private static BalancerAddress[] ConfigureOrderBalancerAddresses()
    {
        string? orderServiceUrls = Environment.GetEnvironmentVariable(OrderUrls);

        if (string.IsNullOrWhiteSpace(orderServiceUrls))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {OrderUrls}. Заполните ее и перезапустите приложение");
        }
        
        return orderServiceUrls.Split(',')
            .Select(url => url.Trim())
            .Select(url => new Uri(url))
            .Select(url => new BalancerAddress(url.Host, url.Port)).ToArray();
    }
}