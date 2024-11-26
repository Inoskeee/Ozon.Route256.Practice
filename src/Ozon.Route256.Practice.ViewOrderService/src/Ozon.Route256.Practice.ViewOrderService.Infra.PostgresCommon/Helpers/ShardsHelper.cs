namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Helpers;

public static class ShardsHelper
{
    public const string BucketPlaceholder = "__bucket__";
    public static string GetSchemaName(int bucketId) => $"bucket_{bucketId}";
}