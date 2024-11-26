using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(2024092102)]
public class CreateTopUpTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists top_up_operations
            (
                guid uuid not null primary key,
                client_id bigserial references clients(client_id) on delete cascade,
                currency_code varchar(3) not null,
                units bigint not null,
                nanos int not null check (nanos between -999999999 and 999999999),
                operation_type int not null,
                operation_status int not null,
                operation_time timestamp not null default now()
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists top_up_operations;");
    }
}