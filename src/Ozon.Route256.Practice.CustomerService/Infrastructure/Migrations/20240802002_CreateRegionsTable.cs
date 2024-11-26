using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

[Migration(20240802002)]
public class CreateRegionsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE IF NOT EXISTS regions
            (
                id   bigint  NOT NULL PRIMARY KEY,
                name text    NOT NULL
            );");
            
        Execute.Sql(@"
            INSERT INTO regions (id, name) 
            VALUES 
                (1, 'Москва'), 
                (2, 'Санкт-Петербург'), 
                (3, 'Екатеренбург');");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS regions;");
    }
}