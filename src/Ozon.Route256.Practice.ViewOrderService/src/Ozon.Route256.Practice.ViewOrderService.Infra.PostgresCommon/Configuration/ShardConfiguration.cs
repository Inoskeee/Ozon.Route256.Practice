namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

public record ShardConfiguration
{
    public required int BucketsCount { get; init; }
    
    public required IReadOnlyList<DatabaseEndpoint> Endpoints { get; init; }
}