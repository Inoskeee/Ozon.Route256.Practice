using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.Enums;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Services;

public class BalanceOperationsDbProvider : IBalanceOperationsDbProvider
{
    private readonly IClientBalanceUnitOfWork _unitOfWork;

    public BalanceOperationsDbProvider(
        IClientBalanceUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultEntity<string?>> CreateTopUpOperation(TopUpOperationEntity operation,
        CancellationToken token)
    {
        return await _unitOfWork.TopUpOperationRepository.Insert(operation, token);
    }

    public async Task<ResultEntity<string?>> CreateWithdrawOperation(WithdrawOperationEntity operation,
        CancellationToken token)
    {
        return await _unitOfWork.WithdrawOperationRepository.Insert(operation, token);
    }

    public async Task<ResultEntity<string?>> ChangeTopUpOperation(TopUpOperationEntity operation,
        CancellationToken token)
    {
        var operationEntity = await _unitOfWork.TopUpOperationRepository.TryGet(operation.Guid, token);
        if (operationEntity is not null)
            switch (operation.OperationStatus)
            {
                case OperationStatusesEntity.Pending or OperationStatusesEntity.Cancelled
                    or OperationStatusesEntity.Reject:
                {
                    return await _unitOfWork.TopUpOperationRepository.Update(operation, token);
                }
                case OperationStatusesEntity.Completed:
                {
                    var clientBalanceEntity = new ClientBalanceEntity(operationEntity.ClientId,
                        operationEntity.CurrencyCode, operationEntity.Units, operationEntity.Nanos);

                    var completedResult = await _unitOfWork.ClientBalanceRepository.UpdateOrInsert(
                        clientBalanceEntity, true, token);

                    if (completedResult.IsSuccess)
                    {
                        return await _unitOfWork.TopUpOperationRepository.Update(operation, token);
                    }

                    var rejectedOperation = new TopUpOperationEntity
                    {
                        Guid = operation.Guid,
                        ClientId = operation.ClientId,
                        CurrencyCode = operation.CurrencyCode,
                        Nanos = operation.Nanos,
                        Units = operation.Units,
                        OperationStatus = OperationStatusesEntity.Reject,
                        OperationTypeEntity = operation.OperationTypeEntity,
                        OperationTime = operation.OperationTime
                    };

                    var resultRejecting = await _unitOfWork.TopUpOperationRepository.Update(rejectedOperation, token);

                    return resultRejecting.IsSuccess
                        ? new ResultEntity<string?>(
                            true,
                            null,
                            $"Операция была отклонена. {completedResult.Message}")
                        : new ResultEntity<string?>(
                            false,
                            null,
                            resultRejecting.Message);
                }
                default:
                    return new ResultEntity<string?>(
                        false,
                        null,
                        "Некорректный статус операции");
            }

        return new ResultEntity<string?>(
            false,
            null,
            $"Не найдена операция с id : {operation.Guid}");
    }

    public async Task<ResultEntity<string?>> ChangeWithdrawOperation(WithdrawOperationEntity operation,
        CancellationToken token)
    {
        var operationEntity = await _unitOfWork.WithdrawOperationRepository.TryGet(operation.Guid, token);
        if (operationEntity is not null)
            switch (operation.OperationStatus)
            {
                case OperationStatusesEntity.Pending or OperationStatusesEntity.Cancelled
                    or OperationStatusesEntity.Reject:
                {
                    return await _unitOfWork.WithdrawOperationRepository.Update(operation, token);
                }
                case OperationStatusesEntity.Completed:
                {
                    var clientBalanceEntity = new ClientBalanceEntity(operationEntity.ClientId,
                        operationEntity.CurrencyCode, operationEntity.Units, operationEntity.Nanos);

                    var completedResult = await _unitOfWork.ClientBalanceRepository.UpdateOrInsert(
                        clientBalanceEntity, false, token);

                    if (completedResult.IsSuccess)
                    {
                        return await _unitOfWork.WithdrawOperationRepository.Update(operation, token);
                    }

                    var rejectedOperation = new WithdrawOperationEntity
                    {
                        Guid = operation.Guid,
                        ClientId = operation.ClientId,
                        CurrencyCode = operation.CurrencyCode,
                        Nanos = operation.Nanos,
                        Units = operation.Units,
                        OperationStatus = OperationStatusesEntity.Reject,
                        OperationTypeEntity = operation.OperationTypeEntity,
                        OperationTime = operation.OperationTime
                    };

                    var resultRejecting =
                        await _unitOfWork.WithdrawOperationRepository.Update(rejectedOperation, token);

                    return resultRejecting.IsSuccess
                        ? new ResultEntity<string?>(
                            true,
                            null,
                            $"Операция была отклонена. {completedResult.Message}")
                        : new ResultEntity<string?>(
                            false,
                            null,
                            resultRejecting.Message);
                }
                default:
                    return new ResultEntity<string?>(
                        false,
                        null,
                        "Некорректный статус операции");
            }

        return new ResultEntity<string?>(
            false,
            null,
            $"Не найдена операция с id : {operation.Guid}");
    }
}