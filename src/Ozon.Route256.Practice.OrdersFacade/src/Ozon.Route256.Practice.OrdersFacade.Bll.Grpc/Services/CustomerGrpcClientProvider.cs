using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.CustomerService;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services;

internal sealed class CustomerGrpcClientProvider : ICustomerGrpcClientProvider
{
    private readonly ILogger<CustomerGrpcClientProvider> _logger;
    private readonly CustomerService.CustomerService.CustomerServiceClient _customerServiceClient;
    
    public CustomerGrpcClientProvider(
        ILogger<CustomerGrpcClientProvider> logger, 
        CustomerService.CustomerService.CustomerServiceClient customerServiceClient)
    {
        _logger = logger;
        _customerServiceClient = customerServiceClient;
    }

    public async Task<List<V1QueryCustomersResponse>> GetCustomersAsync(long[] customerId, int limit, int offset)
    {
        var customerRequest = new V1QueryCustomersRequest()
        {
            CustomerIds = { customerId },
            Limit = limit,
            Offset = offset,
            RegionIds = { }
        };
        var responseCustomer = _customerServiceClient.V1QueryCustomers(customerRequest);
        var customers = new List<V1QueryCustomersResponse>();
        while (await responseCustomer.ResponseStream.MoveNext())
        {
            var currentCustomer = responseCustomer.ResponseStream.Current;
            customers.Add(currentCustomer);
        }
        return customers;
    }
}