using Ozon.Route256.Practice.CustomerService;

namespace Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Services;

public interface ICustomerGrpcClientProvider
{
    Task<List<V1QueryCustomersResponse>> GetCustomersAsync(long[] customerId, int limit, int offset);
}