namespace Ozon.Route256.Practice.ClientBalance.Bll.Models.ResultModels;

public record ResultModel<T>(
    bool IsSuccess,
    ActionSuccessModel<T>? ActionSuccessEntity,
    ActionErrorModel? ErrorEntity);