using FluentMigrator;

namespace Ozon.Route256.OrderService.Migrations;

/// <summary>
/// У типа Money имеется currency_code, отвечающий за валюту,
/// И я посчитал, что деньги пользователя могут храниться в разных валютах
/// Поэтому выделил баланс клиента в отдельную таблицу с составным ключом
/// </summary>
[Migration(2024092101)]
public class CreateBalancesTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            create table if not exists client_balances
            (
                client_id bigserial references clients(client_id) on delete cascade,
                currency_code varchar(3) not null,
                units bigint not null,
                nanos int not null check (nanos between -999999999 and 999999999),
                primary key(client_id, currency_code)
            );");
    }

    public override void Down()
    {
        Execute.Sql("drop table if exists client_balances;");
    }
}