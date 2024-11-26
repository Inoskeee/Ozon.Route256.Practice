namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;

public interface IShardConnectionFactory
{
    string GetConnectionString(int bucketId);
    
    IEnumerable<int> GetAllBuckets();
}