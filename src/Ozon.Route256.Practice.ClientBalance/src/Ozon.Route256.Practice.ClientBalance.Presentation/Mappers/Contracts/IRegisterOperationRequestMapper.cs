using Ozon.Route256.Practice.ClientBalance.Bll.Models;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

public interface IRegisterOperationRequestMapper
{
    RegisterOperationModel MapTopUpRequestToModel(TopUpBalanceRequest topUpBalanceRequest);
    RegisterOperationModel MapWithdrawRequestToModel(WithdrawBalanceRequest withdrawBalanceRequest);
}