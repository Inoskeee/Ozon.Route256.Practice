using Ozon.Route256.Practice.ClientOrders.Bll.Models;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;

public interface IKafkaProvider
{
    public Task<bool> Publish(OrderInputModel message, CancellationToken token);
}