using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Bll.Models.ResultModels;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Services;

public interface IClientBalanceService
{
    Task<ResultModel<string>> CreateClientAsync(
        ClientModel clientModel,
        CancellationToken token);
    Task<ResultModel<string>> TopUpBalanceAsync(
        RegisterOperationModel topUpOperation,
        CancellationToken token);
    Task<ResultModel<string>> WithdrawBalanceAsync(
        RegisterOperationModel withdrawOperation,
        CancellationToken token);
    Task<ResultModel<string>> ChangeOperationStatusAsync(
        ChangeOperationModel operationModel,
        CancellationToken token);
    Task<ResultModel<MoneyModel[]>> GetCurrentBalanceAsync(
        long clientId, 
        CancellationToken token);
}