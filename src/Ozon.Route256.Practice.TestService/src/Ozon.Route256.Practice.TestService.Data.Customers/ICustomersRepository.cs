namespace Ozon.Route256.TestService.Data.Customers;

public interface ICustomersRepository
{
    Task<Customer?> GetCustomerByIdAsync(long customerId, CancellationToken cancellationToken);
}
