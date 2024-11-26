using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;

public class ShardConnectionFactory: IShardConnectionFactory
{
    private readonly IDbStore _dbStore;
    private readonly HashSet<string> _reloadedConnections = new();
    
    public ShardConnectionFactory(
        IDbStore dbStore)
    {
        _dbStore   = dbStore;
    }
    
    public IEnumerable<int> GetAllBuckets()
    {
        for (int bucketId = 0; bucketId < _dbStore.BucketsCount; bucketId++)
        {
            yield return bucketId;
        }
    }

    public string GetConnectionString(int bucketId)
    {
        var endpoint = _dbStore.GetEndpointByBucket(bucketId);

        return endpoint.ConnectionString;
    }
}