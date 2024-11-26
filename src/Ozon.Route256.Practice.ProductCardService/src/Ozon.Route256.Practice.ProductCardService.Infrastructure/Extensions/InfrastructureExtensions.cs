using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ProductCardService.Application.Contracts.External;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Mappers;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Mappers;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Redis;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Redis.Repositories;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Services;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    private const string RedisConnectionString = "ROUTE256_PRODUCT_CARD_SERVICE_REDIS_CONNECTION_STRINGS";

    public static void ConfigureInfrastructureLayer(this IServiceCollection services)
    {
        services.ConfigureMappers();
        services.ConfigureRedis();
    }

    private static void ConfigureRedis(this IServiceCollection services)
    {
        var redisConnectionString = Environment.GetEnvironmentVariable(RedisConnectionString);

        if (string.IsNullOrEmpty(redisConnectionString))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {RedisConnectionString}. Заполните ее и перезапустите приложение");
        }
        
        services.AddScoped<IRedisDatabaseFactory>(_ => new RedisDatabaseFactory(redisConnectionString));
        services.AddScoped<IProductRedisRepository, ProductRedisRepository>();
        services.AddScoped<IRedisProvider, RedisProvider>();
    }
    
    private static void ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IProductMapper, ProductMapper>();
    }
}