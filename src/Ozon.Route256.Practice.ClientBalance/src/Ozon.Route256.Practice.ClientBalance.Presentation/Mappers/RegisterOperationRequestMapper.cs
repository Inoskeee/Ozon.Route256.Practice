using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers;

public class RegisterOperationRequestMapper : IRegisterOperationRequestMapper
{
    public RegisterOperationModel MapTopUpRequestToModel(TopUpBalanceRequest topUpBalanceRequest)
    {
        return new RegisterOperationModel(
            topUpBalanceRequest.Guid,
            topUpBalanceRequest.ClientId,
            Amount: new MoneyModel(
                CurrencyCode: topUpBalanceRequest.TopUpAmount.CurrencyCode,
                Nanos: topUpBalanceRequest.TopUpAmount.Nanos,
                Units: topUpBalanceRequest.TopUpAmount.Units),
            topUpBalanceRequest.OperationTime.ToDateTimeOffset());
    }

    public RegisterOperationModel MapWithdrawRequestToModel(WithdrawBalanceRequest withdrawBalanceRequest)
    {
        return new RegisterOperationModel(
            withdrawBalanceRequest.Guid,
            withdrawBalanceRequest.ClientId,
            Amount: new MoneyModel(
                CurrencyCode: withdrawBalanceRequest.WithdrawAmount.CurrencyCode,
                Nanos: withdrawBalanceRequest.WithdrawAmount.Nanos,
                Units: withdrawBalanceRequest.WithdrawAmount.Units),
            withdrawBalanceRequest.OperationTime.ToDateTimeOffset());
    }
}