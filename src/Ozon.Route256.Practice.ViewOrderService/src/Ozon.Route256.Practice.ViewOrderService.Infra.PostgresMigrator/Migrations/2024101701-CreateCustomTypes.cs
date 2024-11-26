using FluentMigrator;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations.Settings;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations;

[Migration(01, "Create custom types")]
public class CreateCustomTypes : ShardSqlMigration
{
    protected override string GetUpSql(IServiceProvider services)
        => """
            set search_path to public;
            
            do $$
                begin
                    if not exists (select 1 from pg_type where typname = 'order_type') then
                        create type order_type as
                        (
                            order_id bigint,  
                            region_id bigint,
                            status int,
                            customer_id bigint,
                            comment text,
                            created_at timestamptz
                        );
                    end if;
                end
            $$
           """;

    protected override string GetDownSql(IServiceProvider services)
        => """
           set search_path to public;
           
           drop type if exists order_type;
           """;
}