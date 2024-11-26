using System.Text.Json;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Services;

namespace Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Services;

internal sealed class OrdersServiceProvider : IOrdersServiceProvider
{
    private readonly IOrderGrpcClientProvider _clientProvider;

    private readonly IOrderMapper _orderMapper;
    
    public OrdersServiceProvider(
        IOrderGrpcClientProvider clientProvider, 
        IOrderMapper orderMapper)
    {
        _clientProvider = clientProvider;
        _orderMapper = orderMapper;
    }
    
    public async Task<OrderModel?> GetOrder(long orderId, string comment)
    {
        var orderResponse = await _clientProvider.GetOrderByIdAsync(orderId, 1, 0);
        
        if (orderResponse.Count > 0)
        {
            foreach (var order in orderResponse)
            {
                try
                {
                    CommentModel orderComment = JsonSerializer.Deserialize<CommentModel>(order.Comment)!;
                                    
                    if (orderComment.Comment == comment)
                    {
                        var orderModel = _orderMapper.MapOrderDtoToModel(orderResponse.First());
                        return orderModel;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        
        return null;
    }
}