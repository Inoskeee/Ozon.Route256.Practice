using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;

public interface IPostgresProvider
{
    Task<OrderModel[]> GetOrdersByCustomerId(long customerId, CancellationToken cancellationToken);
    
    Task<OrderModel[]> GetOrdersByOrderId(long orderId, CancellationToken cancellationToken);
    
    Task<ResultModel<string>> UpdateOrInsertOrder(OrderModel order, CancellationToken cancellationToken);
    
    Task<ResultModel<string>> RemoveOrder(long orderId, CancellationToken cancellationToken);
}