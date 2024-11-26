using System.Text.Json;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Configuration;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpRequests;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;
using RestSharp;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services;

internal sealed class CustomerHttpClientProvider : ICustomerHttpClientProvider
{
    private readonly IOptions<CustomerServiceConfiguration> _customerServiceConfig;
    public CustomerHttpClientProvider(
        IOptions<CustomerServiceConfiguration> customerServiceConfig)
    {
        _customerServiceConfig = customerServiceConfig;
    }
    public async Task<CustomersHttpResponseDto> GetCustomersAsync(long[] customerIds, int limit, int offset)
    {
        var requestDto = new CustomersHttpRequestDto
        {
            CustomerIds = customerIds.ToList(),
            Limit = limit,
            Offset = offset
        };
        
        var client = new RestClient(_customerServiceConfig.Value.Url);
        
        var request = new RestRequest(_customerServiceConfig.Value.Endpoint, Method.Post);

        request.AddJsonBody(requestDto);

        try
        {
            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new CustomerBadRequestException(
                    $"Error while executing the query: {response.StatusCode} - {response.ErrorMessage}");
            }

            var responseDto = JsonSerializer.Deserialize<CustomersHttpResponseDto>(response.Content!);

            return responseDto!;
        }
        catch (CustomerBadRequestException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CustomerInternalServerException(ex.Message);
        }
    }
}