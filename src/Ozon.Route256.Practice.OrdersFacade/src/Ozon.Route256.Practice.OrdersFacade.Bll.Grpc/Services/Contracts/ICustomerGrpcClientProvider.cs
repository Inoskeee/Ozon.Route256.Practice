using Ozon.Route256.Practice.CustomerService;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;

public interface ICustomerGrpcClientProvider
{
    Task<List<V1QueryCustomersResponse>> GetCustomersAsync(long[] customerId, int limit, int offset);
}