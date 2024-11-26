using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Builders;

public static class DefaultConfigurationBuilder
{
    private const string BucketsPerShard = "ROUTE256_VIEW_ORDER_SERVICE_DB_BUCKETS_PER_SHARD";
    
    private const int BucketsCount = 10;
    
    public static ShardConfiguration Build(string[] connectionStrings)
    {
        var buckets = Enumerable.Range(0, BucketsCount)
            .ToArray();

        var bucketsPerShard = Environment.GetEnvironmentVariable(BucketsPerShard);

        if (string.IsNullOrWhiteSpace(bucketsPerShard) 
            || !int.TryParse(bucketsPerShard, out var bucketsCount))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {BucketsPerShard}. Заполните ее и перезапустите приложение");
        }
        
        var endpoints = new List<DatabaseEndpoint>();
        
        endpoints.Add(new DatabaseEndpoint()
        {
            ConnectionString = connectionStrings[0],
            Buckets = buckets.Take(bucketsCount).ToArray()
        });

        endpoints.Add(new DatabaseEndpoint()
        {
            ConnectionString = connectionStrings[1],
            Buckets = buckets.Skip(bucketsCount).Take(bucketsCount).ToArray()
        });
        
        return new ShardConfiguration()
        {
            BucketsCount = BucketsCount,
            Endpoints = endpoints
        };
    }
}