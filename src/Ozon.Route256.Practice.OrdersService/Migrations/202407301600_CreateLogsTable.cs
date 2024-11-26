using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(202407301600)]
public class CreateLogsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists logs
            (
                id          bigserial                   not null primary key,
                order_id    bigint                      not null,
                region_id   integer                     not null,
                status      integer                     not null,
                customer_id bigint                      not null,
                comment     text,
                created_at  timestamp with time zone    not null,
                updated_at  timestamp with time zone    not null
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists logs;");
    }
}