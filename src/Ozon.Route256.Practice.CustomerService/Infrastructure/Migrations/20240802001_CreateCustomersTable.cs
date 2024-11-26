using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(20240802001)]
public class CreateCustomersTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS customers
            (
                customer_id  bigserial                NOT NULL PRIMARY KEY,
                region_id    bigint                   NOT NULL,
                full_name   text                      NOT NULL,
                created_at  timestamp with time zone  NOT NULL DEFAULT (now() at time zone 'utc'),
                UNIQUE(full_name),
                CHECK (length(full_name) <= 255)
            );");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS CUSTOMERS;");
    }
}