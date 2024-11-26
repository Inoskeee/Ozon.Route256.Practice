using Ozon.Route256.CustomerService.Domain;

namespace Ozon.Route256.CustomerService.Repositories;

public interface ICustomerRepository
{
    Task<long> CreateCustomer(string fullName, long regionId, CancellationToken cancellationToken);
    Task<CustomerQueryModel> GetCustomers(long[] customerIds, long[] regionIds, int limit, int offset, CancellationToken cancellationToken);
}