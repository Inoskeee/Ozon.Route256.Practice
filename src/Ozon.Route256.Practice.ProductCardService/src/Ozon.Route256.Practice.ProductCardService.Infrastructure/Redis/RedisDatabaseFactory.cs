using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;
using StackExchange.Redis;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Redis;

internal sealed class RedisDatabaseFactory(string connectionString) : IRedisDatabaseFactory
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

    public IDatabase GetDatabase()
    {
        return _connectionMultiplexer.GetDatabase();
    }

    public IServer GetServer()
    {
        var endponts = _connectionMultiplexer.GetEndPoints();
        return _connectionMultiplexer.GetServer(endponts.First());
    }
}