using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Contracts;

public interface IClientOrdersService
{
    Task<ResultModel<string>> CreateOrder(long customerId, ItemModel[] items);

    Task<ResultModel<OrderModel[]>> ReceiveOrders(long customerId, int limit, int offset);
}