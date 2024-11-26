using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Extensions;

public static class GrpcClientExtensions
{
    private const string OrderUrls = "ROUTE256_ORDER_SERVICE_GRPC_URLS";

    public static IServiceCollection ConfigureOrderGrpcClient(this IServiceCollection services)
    {
        var orderFactory = new StaticResolverFactory(orderSerivce =>
            ConfigureOrderBalancerAddresses());

        services.AddSingleton<ResolverFactory>(_ => orderFactory);

        services.AddGrpcClient<OrderGrpc.OrderGrpcClient>(options =>
        {
            options.Address = new Uri("static://orderSerivce");
        }).ConfigureChannel(x =>
        {
            x.Credentials = ChannelCredentials.Insecure;
            x.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
        });

        return services;
    }

    private static BalancerAddress[] ConfigureOrderBalancerAddresses()
    {
        var orderServiceUrls = Environment.GetEnvironmentVariable(OrderUrls);

        if (string.IsNullOrWhiteSpace(orderServiceUrls))
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {OrderUrls}. Заполните ее и перезапустите приложение");

        return orderServiceUrls.Split(',')
            .Select(url => url.Trim())
            .Select(url => new Uri(url))
            .Select(url => new BalancerAddress(url.Host, url.Port)).ToArray();
    }
}