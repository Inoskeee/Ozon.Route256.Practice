using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Configuration;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpRequests;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;
using RestSharp;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services;

internal sealed class OrderHttpClientProvider : IOrderHttpClientProvider
{
    private readonly ILogger<OrderHttpClientProvider> _logger;
    private readonly IOptions<OrderServiceConfiguration> _orderServiceConfig;

    public OrderHttpClientProvider(
        ILogger<OrderHttpClientProvider> logger, 
        IOptions<OrderServiceConfiguration> orderServiceConfig)
    {
        _logger = logger;
        _orderServiceConfig = orderServiceConfig;
    }

    public async Task<List<OrdersHttpResponseDto>> GetOrdersByCustomerAsync(long customerId, int limit, int offset)
    {
        var requestDto = new OrdersHttpRequestDto()
        {
            CustomerIds = new List<long>() { customerId },
            Limit = limit,
            Offset = offset
        };
        
        var client = new BalancedRestClient(_orderServiceConfig.Value.Urls);
        
        var request = new RestRequest(_orderServiceConfig.Value.Endpoint, Method.Post);

        request.AddJsonBody(requestDto);
        
        var response = await client.ExecuteAsync(request, _orderServiceConfig.Value.Endpoint);

        var orders = new List<OrdersHttpResponseDto>();

        if (response.IsSuccessful)
        {
            var content = response.Content;
            if (!string.IsNullOrEmpty(content))
            {
                var jsonObjects = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var json in jsonObjects)
                {
                    try
                    {
                        var order = JsonSerializer.Deserialize<OrdersHttpResponseDto>(json);

                        if (order != null)
                        {
                            orders.Add(order);
                        }
                    }
                    catch (JsonException ex)
                    {
                        throw new OrderInternalServerException(ex.Message);
                    }
                }
            }
            return orders;
        }
        else
        {
            throw new OrderBadRequestException(
                $"Error while executing the query: {response.StatusCode} - {response.ErrorMessage}");
        }
    }
    
    public async Task<List<OrdersHttpResponseDto>> GetOrdersByRegionAsync(long regionId, int limit, int offset)
    {
        var requestDto = new OrdersHttpRequestDto()
        {
            RegionIds = new List<long>() { regionId },
            Limit = limit,
            Offset = offset
        };
        
        var client = new BalancedRestClient(_orderServiceConfig.Value.Urls);
        
        var request = new RestRequest(_orderServiceConfig.Value.Endpoint, Method.Post);

        request.AddJsonBody(requestDto);
        
        var response = await client.ExecuteAsync(request, _orderServiceConfig.Value.Endpoint);

        var orders = new List<OrdersHttpResponseDto>();

        if (response.IsSuccessful)
        {
            var content = response.Content;
            if (!string.IsNullOrEmpty(content))
            {
                var jsonObjects = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var json in jsonObjects)
                {
                    try
                    {
                        var order = JsonSerializer.Deserialize<OrdersHttpResponseDto>(json);

                        if (order != null)
                        {
                            orders.Add(order);
                        }
                    }
                    catch (JsonException ex)
                    {
                        throw new OrderInternalServerException(ex.Message);
                    }
                }
            }
            return orders;
        }
        else
        {
            throw new OrderBadRequestException(
                $"Error while executing the query: {response.StatusCode} - {response.ErrorMessage}");
        }
    }
}