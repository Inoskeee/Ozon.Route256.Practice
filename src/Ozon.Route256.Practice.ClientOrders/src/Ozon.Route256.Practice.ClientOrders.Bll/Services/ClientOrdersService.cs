using System.Text.Json;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ClientOrders.Bll.Configuration;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Services;

internal sealed class ClientOrdersService(
    IKafkaProvider kafkaProvider,
    IPostgresProvider postgresProvider,
    ICustomerServiceProvider customerServiceProvider,
    IOptions<ClientOrdersSettings> clientOrdersSettings)
    : IClientOrdersService
{
    public async Task<ResultModel<string>> CreateOrder(long customerId, ItemModel[] items)
    {
        var region = await customerServiceProvider.GetCustomerRegion(customerId);

        CommentModel comment = new CommentModel(
            CustomerId: customerId,
            Comment: clientOrdersSettings.Value.ServiceIdentificator);
        
        OrderInputModel orderInputModel = new OrderInputModel(
            RegionId: region.Id,
            CustomerId: customerId,
            Comment: JsonSerializer.Serialize(comment),
            Items: items);

        var producerResult = await kafkaProvider.Publish(orderInputModel, CancellationToken.None);

        ActionSuccessModel<string>? successResult =
            producerResult
                ? new ActionSuccessModel<string>(SuccessContent: "Успешная отправка сообщения о заказе")
                : null;

        ActionErrorModel? errorModel = 
            producerResult
            ? null
            : new ActionErrorModel(ErrorCode: "PRODUCER_ERROR", ErrorMessage: "Ошибка при отправке сообщения о заказе");

        return new ResultModel<string>(
            IsSuccess: producerResult,
            SuccessEntity: successResult,
            ErrorEntity: errorModel);
    }

    public async Task<ResultModel<OrderModel[]>> ReceiveOrders(long customerId, int limit, int offset)
    {
        var orders = await postgresProvider.GetOrdersByCustomerId(customerId, CancellationToken.None);
        
        ActionSuccessModel<OrderModel[]> successResult = new ActionSuccessModel<OrderModel[]>(SuccessContent: orders);

        return new ResultModel<OrderModel[]>(
            IsSuccess: true,
            SuccessEntity: successResult,
            ErrorEntity: null);
    }
}