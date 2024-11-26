using Ozon.Route256.DataGenerator.Bll.Models;

namespace Ozon.Route256.DataGenerator.Bll.Services.Contracts;

public interface IBrokenOrderService
{
    IReadOnlyList<int> GetBrokenOrderIndexes(
        long totalOrdersCount,
        int invalidOrderCounterNumber,
        int ordersCountToCreate);

    Order BreakOrder(Order order);
}
