using FluentMigrator;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations.Settings;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations;

[Migration(00, "Create sharded tabled")]
public class CreateOrdersTable : ShardSqlMigration
{
    protected override string GetUpSql(IServiceProvider services)
        => """
           create table if not exists orders
            (
              order_id bigint primary key,  
              region_id bigint not null,
              status int not null,
              customer_id bigint not null,
              comment text not null,
              created_at timestamptz not null
            );
        """;
    
    protected override string GetDownSql(IServiceProvider services)
        => """
            drop table if exists orders;
        """;
}