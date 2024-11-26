using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.Enums;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Mappers;

internal sealed class ChangeOperationMapper : IChangeOperationMapper
{
    public TopUpOperationEntity MapOperationToTopUpEntity(ChangeOperationModel changeOperationModel)
    {
        return new TopUpOperationEntity()
        {
            Guid = Guid.Parse(changeOperationModel.Guid),
            ClientId = changeOperationModel.ClientId,
            OperationStatus = (OperationStatusesEntity)changeOperationModel.OperationStatus,
            OperationTypeEntity = (OperationTypesEntity)changeOperationModel.OperationType,
            OperationTime = changeOperationModel.OperationTime
        };
    }

    public WithdrawOperationEntity MapOperationToWithdrawEntity(ChangeOperationModel changeOperationModel)
    {
        return new WithdrawOperationEntity()
        {
            Guid = Guid.Parse(changeOperationModel.Guid),
            ClientId = changeOperationModel.ClientId,
            OperationStatus = (OperationStatusesEntity)changeOperationModel.OperationStatus,
            OperationTypeEntity = (OperationTypesEntity)changeOperationModel.OperationType,
            OperationTime = changeOperationModel.OperationTime
        };
    }
}