using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.DataGenerator.Bll.Mediator.Commands;
using Ozon.Route256.DataGenerator.Bll.Services;
using Ozon.Route256.DataGenerator.Bll.Services.Contracts;

namespace Ozon.Route256.DataGenerator.Bll.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        return services
            .AddServices()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GenerateOrdersCommand).Assembly));
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICustomerService, Services.CustomerService>()
            .AddTransient<IOrderService, OrderService>()
            .AddTransient<IBrokenOrderService, BrokenOrderService>();
    }
}
