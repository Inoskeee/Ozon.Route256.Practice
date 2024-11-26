using Ozon.Route256.DataGenerator.Bll.Models;

namespace Ozon.Route256.DataGenerator.Bll.ProviderContracts;

public interface ICustomerProvider
{
    Task<long?> CreateCustomer(Customer customer, CancellationToken token);
}
