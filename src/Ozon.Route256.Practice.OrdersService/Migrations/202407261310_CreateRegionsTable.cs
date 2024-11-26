using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(202407261310)]
public class CreateRegionsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists regions
            (
                id   bigserial  not null primary key,
                name text       not null
            );");
            
        Execute.Sql(@"
            insert into regions 
                (name) 
            values 
                ('Москва'), 
                ('Санкт-Петербург'), 
                ('Екатеренбург');");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists regions;");
    }
}