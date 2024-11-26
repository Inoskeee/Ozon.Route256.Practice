using Ozon.Route256.OrderService.Bll.Models;

namespace Ozon.Route256.OrderService.Bll.Services.Interfaces;

public interface IOrderService
{
    Task CreateOrder(
        InputOrder inputOrder,
        CancellationToken token);

    Task<(Order[] Orders, int TotalCount)>  QueryOrders(
        long[] orderIds,
        long[] customerIds,
        long[] regionIds,
        int limit,
        int offset,
        CancellationToken token);

    Task<Result> CancelOrder(
        long orderId, 
        CancellationToken token);

    Task<Result> DeliveryOrder(
        long orderId,
        CancellationToken token);
}