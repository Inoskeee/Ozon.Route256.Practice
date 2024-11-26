namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

public sealed record DatabaseEndpoint
{
    public required string ConnectionString { get; init; }
    
    public required int[] Buckets { get; init; }
}