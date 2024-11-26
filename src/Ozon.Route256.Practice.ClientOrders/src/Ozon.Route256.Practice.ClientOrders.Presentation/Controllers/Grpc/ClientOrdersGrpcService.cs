using System.Text.Json;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Controllers.Grpc;

public class ClientOrdersGrpcService : ClientOrdersGrpc.ClientOrdersGrpcBase
{
    private readonly ILogger<ClientOrdersGrpcService> _logger;

    private readonly IClientOrdersService _clientOrdersService;

    private readonly IItemsMapper _itemsMapper;
    private readonly IOrdersMapper _ordersMapper;
    
    public ClientOrdersGrpcService(
        ILogger<ClientOrdersGrpcService> logger, 
        IClientOrdersService clientOrdersService, 
        IItemsMapper itemsMapper, 
        IOrdersMapper ordersMapper)
    {
        _logger = logger;
        _clientOrdersService = clientOrdersService;
        _itemsMapper = itemsMapper;
        _ordersMapper = ordersMapper;
    }

    public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Поступил запрос на создание заказа: {requestBody}", 
            JsonSerializer.Serialize(request));

        ItemModel[] items = _itemsMapper.MapItemsDtoToModel(request.Items);

        var queryResult = await _clientOrdersService.CreateOrder(request.CustomerId, items);
        
        var successResponse = queryResult.IsSuccess
            ? new CreateOrderResponse.Types.CreateOrderSuccess()
            {
                Message = queryResult.SuccessEntity!.SuccessContent
            }
            : null;
        
        var errorResponse = queryResult.IsSuccess
            ? null
            : new CreateOrderResponse.Types.CreateOrderError()
            {
                Code = queryResult.ErrorEntity!.ErrorCode,
                Message = queryResult.ErrorEntity.ErrorMessage
            };
        
        return 
            queryResult.IsSuccess
            ? new CreateOrderResponse()
                {
                    Ok = successResponse
                }
            : new CreateOrderResponse()
                {
                    Error = errorResponse
                };
    }

    public override async Task<GetOrdersResponse> GetOrders(GetOrdersRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Поступил запрос на получение заказов: {requestBody}", 
            JsonSerializer.Serialize(request));

        var queryResult = await _clientOrdersService.ReceiveOrders(request.CustomerId, request.Limit, request.Offset);

        if (queryResult.IsSuccess)
        {
            var customerOrders = _ordersMapper.MapOrderModelToDto(
                queryResult.SuccessEntity!.SuccessContent);
            
            return new GetOrdersResponse()
            {
                CustomerId = request.CustomerId,
                Orders = { customerOrders }
            };
        }
        
        return new GetOrdersResponse()
        {
            CustomerId = request.CustomerId,
            Orders = { }
        };
        
    }
}