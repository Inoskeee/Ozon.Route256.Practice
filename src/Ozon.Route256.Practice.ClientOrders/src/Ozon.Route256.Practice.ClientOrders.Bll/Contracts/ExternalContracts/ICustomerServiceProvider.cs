using Ozon.Route256.Practice.ClientOrders.Bll.Models.Customer;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;

public interface ICustomerServiceProvider
{
    Task<RegionModel> GetCustomerRegion(long customerId);
}