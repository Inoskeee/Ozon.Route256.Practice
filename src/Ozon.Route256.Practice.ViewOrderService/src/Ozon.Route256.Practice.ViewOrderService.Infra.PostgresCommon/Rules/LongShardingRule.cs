using Murmur;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;

public class LongShardingRule: IShardingRule<long>
{
    private readonly int _bucketsCount;
    
    public LongShardingRule(int bucketsCount)
    {
        _bucketsCount = bucketsCount;
    }
    
    public int GetBucketId(long shardKey)
    {
        var shardKeyHashCode = GetShardKeyHashCode(shardKey);

        return Math.Abs(shardKeyHashCode) % _bucketsCount;
    }

    private int GetShardKeyHashCode(long shardKey)
    {
        var bytes = BitConverter.GetBytes(shardKey);
        var murmur = MurmurHash.Create32();
        var hash = murmur.ComputeHash(bytes);
        return BitConverter.ToInt32(hash);
    }
}