using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;

public class DbStore : IDbStore
{
    private readonly DatabaseEndpoint[] _endpoints;
    private readonly Dictionary<long, DatabaseEndpoint> _bucketEndpoint;

    public DbStore(DatabaseEndpoint[] dbEndpoints)
    {
        _bucketEndpoint = new Dictionary<long, DatabaseEndpoint>();
        int bucketsCount = 0;
        
        foreach (var endpoint in dbEndpoints)
        {
            foreach (var bucket in endpoint.Buckets)
            {
                _bucketEndpoint.Add(bucket, endpoint);
                bucketsCount++;
            }
        }

        _endpoints = dbEndpoints;
        BucketsCount = bucketsCount;
    }

    public IReadOnlyList<DatabaseEndpoint> GetAllEndpoints()
        => _endpoints;
    
    public DatabaseEndpoint GetEndpointByBucket(
        int bucketId)
    {
        if (!_bucketEndpoint.TryGetValue(bucketId, out DatabaseEndpoint? bucket))
        {
            throw new ArgumentOutOfRangeException($"There is not bucket {bucketId}");
        }

        return bucket;
    }

    public int BucketsCount { get; private set; }
}