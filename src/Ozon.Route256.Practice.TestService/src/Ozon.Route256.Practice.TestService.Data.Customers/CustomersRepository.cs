using Ozon.Route256.TestService.Common.Data;

namespace Ozon.Route256.TestService.Data.Customers;

public class CustomersRepository : ICustomersRepository
{
    private readonly IDbFacade _dbFacade;

    public CustomersRepository(IDbFacade<CustomersDataConnectionOptions> dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public async Task<Customer?> GetCustomerByIdAsync(long customerId, CancellationToken cancellationToken)
    {
        var query = CustomersSqlHelper.CreateGetCustomerByIdCommand(customerId, cancellationToken);

        var result = await _dbFacade.QueryAsync<Customer>(query);

        return result.FirstOrDefault();
    }
}
