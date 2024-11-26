using Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;
using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Contracts.Services;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Services;

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
    
    public async Task<OrderModel?> GetOrder(long orderId)
    {
        var orderResponse = await _clientProvider.GetOrderByIdAsync(orderId, 1, 0);
        
        if (orderResponse.Count > 0)
        {
            var orderModel = _orderMapper.MapOrderDtoToModel(orderResponse.First());
            return orderModel;
        }
        
        return null;
    }
}