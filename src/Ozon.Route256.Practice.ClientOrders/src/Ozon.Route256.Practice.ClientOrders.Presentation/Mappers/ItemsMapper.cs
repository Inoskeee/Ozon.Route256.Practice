using Google.Protobuf.Collections;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Mappers;

internal sealed class ItemsMapper : IItemsMapper
{
    public ItemModel[] MapItemsDtoToModel(RepeatedField<CreateOrderRequest.Types.Item> items)
    {
        var itemsList = new ItemModel[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            var itemModel = new ItemModel(
                BarCode: items[i].Barcode,
                Quantity: items[i].Quantity);

            itemsList[i] = itemModel;
        }

        return itemsList;
    }
}