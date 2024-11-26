using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Mappers;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Mappers;

internal sealed class OrderInputMapper : IOrderInputMapper
{
    public OrderInputMessage MapOrderModelToMessage(OrderInputModel orderInputModel)
    {
        Item[] items = new Item[orderInputModel.Items.Length];

        for (int i = 0; i < orderInputModel.Items.Length; i++)
        {
            Item item = new Item()
            {
                Barcode = orderInputModel.Items[i].BarCode,
                Quantity = orderInputModel.Items[i].Quantity
            };
            items[i] = item;
        }
        
        return new OrderInputMessage()
        {
            CustomerId = orderInputModel.CustomerId,
            Comment = orderInputModel.Comment,
            Items = { items },
            RegionId = orderInputModel.RegionId
        };
    }
}