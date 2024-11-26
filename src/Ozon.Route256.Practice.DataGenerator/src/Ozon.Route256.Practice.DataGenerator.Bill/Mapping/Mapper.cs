using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.Messages;
using Item = Ozon.Route256.DataGenerator.Bll.Models.Item;
using MessageItem = Ozon.Route256.DataGenerator.Messages.Item;

namespace Ozon.Route256.DataGenerator.Bll.Mapping;

public static class Mapper
{
    public static OrderInputMessage Map(this Order order)
    {
        return new OrderInputMessage
        {
            RegionId = order.RegionId,
            CustomerId = order.CustomerId,
            Comment = order.Comment,
            Items = { order.Items.Map() }
        };
    }

    private static IReadOnlyList<MessageItem> Map(this IReadOnlyList<Item> items)
    {
        return items
            .Select(item => new MessageItem
            {
                Barcode = item.Barcode,
                Quantity = item.Quantity,
            }).ToArray();
    }
}
