namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;

public interface IShardingRule<TShardKey>
{
    int GetBucketId(TShardKey shardKey);
}