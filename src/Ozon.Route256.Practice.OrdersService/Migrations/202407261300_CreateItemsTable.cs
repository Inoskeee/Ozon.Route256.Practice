using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(202407261300)]
public class CreateItemsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists items
            (
                id          bigserial   not null primary key,
                order_id    bigint      not null,
                barcode     text,
                quantity    integer     not null
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists items;");
    }
}