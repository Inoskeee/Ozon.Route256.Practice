using FluentMigrator;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Migrations;

[Migration(2024100500)]
public class CreateOrdersTable : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            create table if not exists client_orders
            (
                order_id bigserial not null primary key,
                customer_id bigint not null,
                order_status int not null,
                created_at timestamp not null
            );
            """);
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists client_orders;");
    }
}