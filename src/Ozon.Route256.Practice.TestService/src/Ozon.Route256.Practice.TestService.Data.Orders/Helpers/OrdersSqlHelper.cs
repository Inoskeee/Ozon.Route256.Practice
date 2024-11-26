using System.Data;
using Dapper;

namespace Ozon.Route256.TestService.Data.Orders;

public static class OrdersSqlHelper
{
    private const string OrderId = "@order_id";
    private const string RegionId = "@region_id";
    private const string CustomerId = "@customer_id";
    private const string ItemsBarcode = "@items_barcode";
    private const string ItemsQuantity = "@items_quantity";

    public static CommandDefinition CreateGetOrderByIdCommand(long orderId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add(OrderId, orderId, DbType.Int64);

        return new CommandDefinition(
            commandText: $"""
                          select
                              o.order_id as {nameof(Order.OrderId)},
                              o.region_id as {nameof(Order.RegionId)},
                              o.status as {nameof(Order.Status)},
                              o.customer_id as {nameof(Order.CustomerId)},
                              o.comment as {nameof(Order.Comment)},
                              o.created_at as {nameof(Order.CreatedAt)}
                          from orders as o
                          where o.order_id = {OrderId}
                          """,
            parameters: parameters,
            cancellationToken: cancellationToken);
    }

    public static CommandDefinition CreateSelectItemsByOrderIdCommand(long orderId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add(OrderId, orderId, DbType.Int64);

        return new CommandDefinition(
            commandText: $"""
                          select
                              i.id as {nameof(OrderItem.Id)},
                              i.order_id as {nameof(OrderItem.OrderId)},
                              i.barcode as {nameof(OrderItem.Barcode)},
                              i.quantity as {nameof(OrderItem.Quantity)}
                          from items as i
                          where i.order_id = {OrderId}
                          """,
            parameters: parameters,
            cancellationToken: cancellationToken);
    }

    public static CommandDefinition CreateSelectItemsByOrderIdCommand(IReadOnlyCollection<long> orderIds, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add(OrderId, orderIds);

        return new CommandDefinition(
            commandText: $"""
                           select
                               i.id as {nameof(OrderItem.Id)},
                               i.order_id as {nameof(OrderItem.OrderId)},
                               i.barcode as {nameof(OrderItem.Barcode)},
                               i.quantity as {nameof(OrderItem.Quantity)}
                           from items as i
                           where i.order_id = ANY({OrderId}::BIGINT[])
                           """,
            parameters: parameters,
            cancellationToken: cancellationToken);
    }

    public static CommandDefinition CreateSearchOrdersCommand(SearchOrdersQueryParams queryParams, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add(RegionId, queryParams.RegionId, DbType.Int64);
        parameters.Add(CustomerId, queryParams.CustomerId, DbType.Int64);
        parameters.Add(ItemsBarcode, queryParams.GetItemsBarcode());
        parameters.Add(ItemsQuantity, queryParams.GetItemsQuantity());

        return new CommandDefinition(
            commandText: $"""
                           with items_info as (
                               SELECT a.barcode, a.quantity
                               FROM unnest(
                                   {ItemsBarcode}::text[],
                                   {ItemsQuantity}::integer[]
                               ) AS a (barcode, quantity)
                           )
                           select
                               o.order_id as {nameof(Order.OrderId)},
                               o.region_id as {nameof(Order.RegionId)},
                               o.status as {nameof(Order.Status)},
                               o.customer_id as {nameof(Order.CustomerId)},
                               o.comment as {nameof(Order.Comment)},
                               o.created_at as {nameof(Order.CreatedAt)}
                           from orders as o
                           where o.region_id = {RegionId}
                             and o.customer_id = {CustomerId}
                             and o.order_id in (
                                select i.order_id
                                from items as i
                                inner join items_info as ii
                                    on  i.barcode = ii.barcode
                                    and i.quantity = ii.quantity
                             )
                           """,
            parameters: parameters,
            cancellationToken: cancellationToken);
    }
}
