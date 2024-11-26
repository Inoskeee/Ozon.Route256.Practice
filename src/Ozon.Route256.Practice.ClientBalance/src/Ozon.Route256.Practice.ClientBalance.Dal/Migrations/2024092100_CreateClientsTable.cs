using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(2024092100)]
public class CreateClientsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists clients
            (
                client_id bigserial not null primary key,
                client_name varchar(127) not null
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists clients;");
    }
}