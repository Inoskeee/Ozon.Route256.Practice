using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Services;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Services;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Extensions;

public static class OrdersServiceExtensions
{
    private const string OrderServiceUrls = "ROUTE256_ORDER_SERVICE_GRPC_URLS";
    
    public static void ConfigureOrdersServiceInfrastructure(this IServiceCollection services)
    {
        services.ConfigureOrdersServiceMappers();

        services.ConfigureOrdersClient();
        
        services.AddScoped<IOrderGrpcClientProvider, OrderGrpcClientProvider>();
        services.AddScoped<IOrdersServiceProvider, OrdersServiceProvider>();
    }

    private static void ConfigureOrdersServiceMappers(this IServiceCollection services)
    {
        services.AddScoped<IOrderMapper, OrderMapper>();
    }
    
    private static void ConfigureOrdersClient(this IServiceCollection services)
    {
        var orderFactory = new StaticResolverFactory(orderSerivce => 
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
    }
    
    private static BalancerAddress[] ConfigureOrderBalancerAddresses()
    {
        string? orderServiceUrls = Environment.GetEnvironmentVariable(OrderServiceUrls);

        if (string.IsNullOrWhiteSpace(orderServiceUrls))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {OrderServiceUrls}. Заполните ее и перезапустите приложение");
        }
        
        return orderServiceUrls.Split(',')
            .Select(url => url.Trim())
            .Select(url => new Uri(url))
            .Select(url => new BalancerAddress(url.Host, url.Port)).ToArray();
    }
}