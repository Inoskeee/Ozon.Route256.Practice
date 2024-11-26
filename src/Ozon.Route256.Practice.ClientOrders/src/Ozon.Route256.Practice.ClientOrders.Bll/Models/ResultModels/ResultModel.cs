namespace Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

public sealed record ResultModel<T>(
    bool IsSuccess,
    ActionSuccessModel<T>? SuccessEntity,
    ActionErrorModel? ErrorEntity);