using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Services;

internal sealed class PostgresProvider : IPostgresProvider
{
    private readonly IClientOrdersRepository _clientOrdersRepository;

    private readonly IOrderMapper _orderMapper;
    
    public PostgresProvider(
        IClientOrdersRepository clientOrdersRepository, 
        IOrderMapper orderMapper)
    {
        _clientOrdersRepository = clientOrdersRepository;
        _orderMapper = orderMapper;
    }


    public async Task<OrderModel[]> GetOrdersByCustomerId(long customerId, CancellationToken cancellationToken)
    {
        var queryResult = await _clientOrdersRepository.GetByCustomerId(customerId, cancellationToken);

        OrderModel[] orders = new OrderModel[queryResult.Length];

        for (int i = 0; i < queryResult.Length; i++)
        {
            OrderModel orderModel = _orderMapper.MapOrderEntityToModel(queryResult[i]);
            orders[i] = orderModel;
        }

        return orders;
    }

    public async Task<OrderModel[]> GetOrdersByOrderId(long orderId, CancellationToken cancellationToken)
    {
        var queryResult = await _clientOrdersRepository.GetByOrderId(orderId, cancellationToken);

        OrderModel[] orders = new OrderModel[queryResult.Length];

        for (int i = 0; i < queryResult.Length; i++)
        {
            OrderModel orderModel = _orderMapper.MapOrderEntityToModel(queryResult[i]);
            orders[i] = orderModel;
        }

        return orders;
    }

    public async Task<ResultModel<string>> UpdateOrInsertOrder(OrderModel order, CancellationToken cancellationToken)
    {
        OrderEntity orderEntity = _orderMapper.MapOrderModelToEntity(order);
        var queryResult = await _clientOrdersRepository.UpdateOrInsert(orderEntity, cancellationToken);

        ActionSuccessModel<string>? successResult = 
            queryResult.IsSuccess 
            ? new ActionSuccessModel<string>(
                SuccessContent: queryResult.Message)
            : null;

        ActionErrorModel? errorModel =
            queryResult.IsSuccess
                ? null
                : new ActionErrorModel(
                    ErrorCode: "UPSERT_ERROR", 
                    ErrorMessage: queryResult.Message);

        return new ResultModel<string>(
            IsSuccess: queryResult.IsSuccess,
            SuccessEntity: successResult,
            ErrorEntity: errorModel);
    }

    public async Task<ResultModel<string>> RemoveOrder(long orderId, CancellationToken cancellationToken)
    {
        var queryResult = await _clientOrdersRepository.Delete(orderId, cancellationToken);

        ActionSuccessModel<string>? successResult = 
            queryResult.IsSuccess 
                ? new ActionSuccessModel<string>(
                    SuccessContent: queryResult.Message)
                : null;

        ActionErrorModel? errorModel =
            queryResult.IsSuccess
                ? null
                : new ActionErrorModel(
                    ErrorCode: "DELETE_ERROR", 
                    ErrorMessage: queryResult.Message);

        return new ResultModel<string>(
            IsSuccess: queryResult.IsSuccess,
            SuccessEntity: successResult,
            ErrorEntity: errorModel);
    }
}