using StackExchange.Redis;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;

internal interface IRedisDatabaseFactory
{
    IDatabase GetDatabase();

    IServer GetServer();
}