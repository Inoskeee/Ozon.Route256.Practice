using Microsoft.Extensions.Logging;
using Ozon.Route256.CustomerService;
using Ozon.Route256.DataGenerator.Bll.ProviderContracts;
using Customer = Ozon.Route256.DataGenerator.Bll.Models.Customer;
using CustomerServiceClient = Ozon.Route256.CustomerService.CustomerService.CustomerServiceClient;

namespace Ozon.Route256.DataGenerator.Infra.Providers;

public class CustomerProvider : ICustomerProvider
{
    private readonly CustomerServiceClient _customerClient;
    private readonly ILogger<CustomerProvider> _logger;

    public CustomerProvider(
        CustomerServiceClient customerClient,
        ILogger<CustomerProvider> logger)
    {
        _customerClient = customerClient;
        _logger = logger;
    }

    public async Task<long?> CreateCustomer(
        Customer customer,
        CancellationToken token)
    {
        var request = new V1CreateCustomerRequest
        {
            RegionId = customer.RegionId,
            FullName = customer.FullName
        };

        var response = await _customerClient.V1CreateCustomerAsync(
            request,
            cancellationToken: token);

        if (response.Error is not null)
        {
            _logger.LogError(
                "Ошибка при создании пользователя: [{ErrorCode}] - [{ErrorText}]",
                response.Error.Code,
                response.Error.Text);
        }

        return response.ResultCase switch
        {
            V1CreateCustomerResponse.ResultOneofCase.Ok => response.Ok.CustomerId,
            V1CreateCustomerResponse.ResultOneofCase.Error => null,

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
