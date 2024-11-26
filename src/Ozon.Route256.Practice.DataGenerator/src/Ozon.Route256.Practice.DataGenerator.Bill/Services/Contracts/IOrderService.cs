using Ozon.Route256.DataGenerator.Bll.Creators;
using Ozon.Route256.DataGenerator.Bll.Models;
using Random = Ozon.Route256.DataGenerator.Bll.Creators.Random;

namespace Ozon.Route256.DataGenerator.Bll.Services.Contracts;

public interface IOrderService
{
    Order Create(IReadOnlyList<Customer> customers)
    {
        var customer = Random.Element(customers);

        var order = OrderCreator.Create() with
        {
            CustomerId = customer.Id!.Value,
            RegionId = customer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };

        return order;
    }
}
