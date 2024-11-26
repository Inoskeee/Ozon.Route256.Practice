using System.Data;
using Dapper;

namespace Ozon.Route256.TestService.Data.Customers;

public static class CustomersSqlHelper
{
    private const string CustomerId = "@customer_id";

    public static CommandDefinition CreateGetCustomerByIdCommand(long customerId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add(CustomerId, customerId, DbType.Int64);

        return new CommandDefinition(
            commandText: $"""
                          select
                            c.customer_id as {nameof(Customer.Id)},
                            c.region_id as {nameof(Customer.RegionId)},
                            c.full_name as {nameof(Customer.FullName)},
                            c.created_at as {nameof(Customer.CreatedAt)}
                          from customers as c
                          where c.customer_id = {CustomerId}
                          """,
            parameters: parameters,
            cancellationToken: cancellationToken);
    }
}
