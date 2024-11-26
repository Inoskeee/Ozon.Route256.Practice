using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(202407261250)]
public class CreateOrdersTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists orders
            (
                order_id    bigserial                   not null primary key,
                region_id   integer                     not null,
                status      integer                     not null,
                customer_id bigint                      not null,
                comment     text,
                created_at  timestamp with time zone    not null
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists orders;");
    }
}