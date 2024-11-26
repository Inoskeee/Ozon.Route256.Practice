using Google.Protobuf.Collections;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

public interface IItemsMapper
{
    ItemModel[] MapItemsDtoToModel(RepeatedField<CreateOrderRequest.Types.Item> items);
}