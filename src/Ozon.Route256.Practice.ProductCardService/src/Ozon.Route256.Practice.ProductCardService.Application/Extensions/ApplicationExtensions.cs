using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ProductCardService.Application.Contracts;
using Ozon.Route256.Practice.ProductCardService.Application.Services;

namespace Ozon.Route256.Practice.ProductCardService.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}