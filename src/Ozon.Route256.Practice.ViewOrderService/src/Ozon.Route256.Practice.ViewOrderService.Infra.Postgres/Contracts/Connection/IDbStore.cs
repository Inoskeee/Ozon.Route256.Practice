using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;

public interface IDbStore
{
    IReadOnlyList<DatabaseEndpoint> GetAllEndpoints();
    
    DatabaseEndpoint GetEndpointByBucket(int bucketId);
    
    int BucketsCount { get; }
}