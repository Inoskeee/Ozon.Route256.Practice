using Dapper;

using Ozon.Route256.CustomerService.Domain;
using Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories.Contracts;
using Ozon.Route256.CustomerService.Repositories;
using Ozon.Route256.CustomerService.Repositories.Exceptions;

namespace Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public async Task<long> CreateCustomer(string fullName, long regionId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                INSERT INTO customers (region_id, full_name)
                                VALUES 
                                   (@RegionId, @FullName)
                                ON CONFLICT(full_name) DO NOTHING
                                RETURNING customer_id;
                                """;

        var param = new DynamicParameters();
        param.Add("RegionId", regionId);
        param.Add("FullName", fullName);
        var ids = await ExecuteQueryAsync<long?>(sqlQuery, param, cancellationToken);

        if (ids.Length == 0)
        {
            throw new CustomerAlreadyExistsException($"Customer with the '{fullName}' name already exists.");
        }

        return ids.First()!.Value;
    }

    public async Task<CustomerQueryModel> GetCustomers(long[] customerIds, long[] regionIds, int limit, int offset, CancellationToken cancellationToken)
    {
        const string customerCondition = "customer_id = Any(@Customers)";
        const string regionCondition = "region_id = Any(@Regions)";
        var condition = customerIds.Length > 0 && regionIds.Length > 0
            ? $"{customerCondition} AND {regionCondition}"
            : customerIds.Length > 0
                ? customerCondition
                : regionCondition;
        var sqlQuery = $"""
                        SELECT 
                           customer_id,
                           region_id,
                           r.name as region_name,
                           full_name,
                           created_at,
                           count(*) OVER() AS total_count
                        FROM customers c
                        JOIN regions r on c.region_id = r.id
                        WHERE {condition}
                        ORDER BY customer_id
                        LIMIT @Limit
                        OFFSET @Offset
                        
                        """;

            var param = new DynamicParameters();
            param.Add("Customers", customerIds);
            param.Add("Regions", regionIds);
            param.Add("Limit", limit);
            param.Add("Offset", offset);

            var records = await ExecuteQueryAsync<CustomerQueryRecord>(sqlQuery, param, cancellationToken);
            var customers = records.Select(
                    r => new Customer
                    {
                        Id = r.CustomerId,
                        FullName = r.FullName,
                        Region = new Region(r.RegionId, r.RegionName!),
                        CreatedAt = r.CreatedAt
                    })
                .ToArray();

            var totalCount = records.Length == 0 ? 0 : records.First().TotalCount;

            return new CustomerQueryModel(totalCount, customers);
    }
}