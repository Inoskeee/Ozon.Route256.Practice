using Dapper;
using Ozon.Route256.OrderService.Dal.Entities;

namespace Ozon.Route256.OrderService.Dal.Repositories;

public class ItemsRepository : BaseRepository, IItemsRepository
{
    public ItemsRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task Insert(
        ItemEntity[] items,
        CancellationToken token)
    {
        if (items is not { Length: > 0 })
        {
            return;
        }

        const string sqlQuery = @"
                with items_info as (
                    select i.order_id, i.barcode, i.quantity 
                    from unnest(
                        @OrderIds::bigint[],
                        @Barcodes::text[],
                        @Quantities::integer[]
                ) as i (order_id, barcode, quantity))
                insert into 
                    items (order_id, barcode, quantity)
                select 
                    ii.order_id, ii.barcode, ii.quantity
                from 
                    items_info as ii;";

        var param = new DynamicParameters();
        param.Add("OrderIds", items.Select(item => item.OrderId).ToArray());
        param.Add("Barcodes", items.Select(item => item.Barcode).ToArray());
        param.Add("Quantities", items.Select(item => item.Quantity).ToArray());
        
        await ExecuteNonQueryAsync(sqlQuery, param, token);
    }
}