using FluentMigrator.Runner.VersionTableInfo;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations.Settings;

public sealed class ShardVersionTableMetaData: IVersionTableMetaData
{
    private readonly IBucketMigrationContext _context;

    public ShardVersionTableMetaData(
        IBucketMigrationContext context)
    {
        _context = context;
    }

    public object ApplicationContext { get; set; } = null!;
    
    public bool OwnsSchema => true;
    
    public string SchemaName => _context.CurrentDbSchema;
    
    public string TableName => "version_info";
    
    public string ColumnName => "version";
    
    public string DescriptionColumnName => "description";
    
    public string UniqueIndexName => "version_unique_idx";
    
    public string AppliedOnColumnName => "applied_on";
}