using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Services;
using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Bll.Models.Enums;
using Ozon.Route256.Practice.ClientBalance.Bll.Models.ResultModels;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Services;

public class ClientBalanceService : IClientBalanceService
{
    private readonly IClientDbProvider _clientDbProvider;
    private readonly IBalanceOperationsDbProvider _balanceOperationsDbProvider;

    private readonly IClientMapper _clientMapper;
    private readonly IChangeOperationMapper _changeOperationMapper;
    private readonly IRegisterOperationMapper _registerOperationMapper;
    private readonly IClientBalanceMapper _clientBalanceMapper;
    public ClientBalanceService(
        IClientDbProvider clientDbProvider, 
        IBalanceOperationsDbProvider balanceOperationsDbProvider,
        IClientMapper clientMapper, 
        IChangeOperationMapper changeOperationMapper, 
        IRegisterOperationMapper registerOperationMapper, 
        IClientBalanceMapper clientBalanceMapper)
    {
        _clientDbProvider = clientDbProvider;
        _balanceOperationsDbProvider = balanceOperationsDbProvider;
        _clientMapper = clientMapper;
        _changeOperationMapper = changeOperationMapper;
        _registerOperationMapper = registerOperationMapper;
        _clientBalanceMapper = clientBalanceMapper; }

    public async Task<ResultModel<string>> CreateClientAsync(ClientModel clientModel, CancellationToken cancellationToken)
    {
        var clientEntity = _clientMapper.MapModelToEntity(clientModel);
        
        var insertResult = await _clientDbProvider.CreateClient(clientEntity, cancellationToken);
        
        return insertResult is not null ? 
            new ResultModel<string>(IsSuccess: true, 
                ActionSuccessEntity: 
                    new ActionSuccessModel<string>(
                        SuccessContent: "Клиент успешно добавлен."), 
                ErrorEntity: null) : 
            new ResultModel<string>(IsSuccess: false, 
                ActionSuccessEntity: null, 
                ErrorEntity: 
                    new ActionErrorModel(
                        ErrorCode: "CANNOT_ADD_CLIENT",
                        ErrorMessage: $"Не удалось добавить клиента c id: {clientModel.ClientId}."));
    }

    public async Task<ResultModel<string>> TopUpBalanceAsync(RegisterOperationModel topUpOperation, CancellationToken token)
    {
        var topUpEntity = _registerOperationMapper.MapOperationToTopUpEntity(topUpOperation);
        
        var insertResult = await _balanceOperationsDbProvider.CreateTopUpOperation(topUpEntity, token);
        
        return insertResult.IsSuccess ? 
            new ResultModel<string>(IsSuccess: true, 
                ActionSuccessEntity: 
                new ActionSuccessModel<string>(
                    SuccessContent: "Операция успешно зарегистрирована."), 
                ErrorEntity: null) :
            new ResultModel<string>(IsSuccess: false, 
                ActionSuccessEntity: null, 
                ErrorEntity: new ActionErrorModel(
                    ErrorCode: "CANNOT_ADD_OPERATION",
                    ErrorMessage: $"Не удалось зарегистрировать операцию для клиента с id: {topUpOperation.ClientId}. {insertResult.Message}"));
    }

    public async Task<ResultModel<string>> WithdrawBalanceAsync(RegisterOperationModel withdrawOperation, CancellationToken token)
    {
        var withdrawEntity = _registerOperationMapper.MapOperationToWithdrawEntity(withdrawOperation);
        
        var insertResult = await _balanceOperationsDbProvider.CreateWithdrawOperation(withdrawEntity, token);
        
        return insertResult.IsSuccess ? 
            new ResultModel<string>(IsSuccess: true, 
                ActionSuccessEntity: 
                new ActionSuccessModel<string>(
                    SuccessContent: "Операция успешно зарегистрирована."), 
                ErrorEntity: null) :
            new ResultModel<string>(IsSuccess: false, 
                ActionSuccessEntity: null, 
                ErrorEntity: new ActionErrorModel(
                    ErrorCode: "CANNOT_WITHDRAW_OPERATION",
                    ErrorMessage: $"Не удалось зарегистрировать операцию для клиента с id: {withdrawOperation.ClientId}. {insertResult.Message}"));
    }

    public async Task<ResultModel<string>> ChangeOperationStatusAsync(ChangeOperationModel operationModel, CancellationToken token)
    {
        switch (operationModel.OperationType)
        {
            case OperationTypesEnum.TopUp:
            {
                var topUpEntity = _changeOperationMapper.MapOperationToTopUpEntity(operationModel);
        
                var changeOperationResult = await _balanceOperationsDbProvider.ChangeTopUpOperation(topUpEntity, token);
                return changeOperationResult.IsSuccess ? 
                    new ResultModel<string>(IsSuccess: true, 
                        ActionSuccessEntity: 
                        new ActionSuccessModel<string>(
                            SuccessContent: changeOperationResult.Message), 
                        ErrorEntity: null) :
                    new ResultModel<string>(IsSuccess: false, 
                        ActionSuccessEntity: null, 
                        ErrorEntity: new ActionErrorModel(
                            ErrorCode: "CANNOT_CHANGE_OPERATION",
                            ErrorMessage: $"Не удалось изменить операцию с guid: {operationModel.Guid}. {changeOperationResult.Message}"));
            }
            case OperationTypesEnum.Withdraw:
            {
                var withdrawEntity = _changeOperationMapper.MapOperationToWithdrawEntity(operationModel);
        
                var changeOperationResult = await _balanceOperationsDbProvider.ChangeWithdrawOperation(withdrawEntity, token);
                return changeOperationResult.IsSuccess ? 
                    new ResultModel<string>(IsSuccess: true, 
                        ActionSuccessEntity: 
                        new ActionSuccessModel<string>(
                            SuccessContent: changeOperationResult.Message), 
                        ErrorEntity: null) :
                    new ResultModel<string>(IsSuccess: false, 
                        ActionSuccessEntity: null, 
                        ErrorEntity: new ActionErrorModel(
                            ErrorCode: "CANNOT_CHANGE_OPERATION",
                            ErrorMessage: $"Не удалось изменить операцию с guid: {operationModel.Guid}. {changeOperationResult.Message}"));
            }
        }
        return new ResultModel<string>(IsSuccess: false, 
            ActionSuccessEntity: null, 
            ErrorEntity: new ActionErrorModel(
                ErrorCode: "CANNOT_CHANGE_OPERATION",
                ErrorMessage: $"Не удалось изменить операцию с guid: {operationModel.Guid}."));
    }

    public async Task<ResultModel<MoneyModel[]>> GetCurrentBalanceAsync(long clientId, CancellationToken token)
    {
        var balanceResult = await _clientDbProvider.GetClientBalance(clientId, token);

        var mappedResult = _clientBalanceMapper.MapEntityToModel(balanceResult);

        return new ResultModel<MoneyModel[]>(
            IsSuccess: true,
            ActionSuccessEntity: new ActionSuccessModel<MoneyModel[]>(
                mappedResult),
            ErrorEntity: null);
    }
}