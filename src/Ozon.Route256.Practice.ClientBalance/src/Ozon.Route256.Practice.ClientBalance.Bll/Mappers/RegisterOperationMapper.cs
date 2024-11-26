using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.Enums;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Mappers;

internal sealed class RegisterOperationMapper : IRegisterOperationMapper
{
    public TopUpOperationEntity MapOperationToTopUpEntity(RegisterOperationModel registerOperationModel)
    {
        return new TopUpOperationEntity()
        {
            Guid = Guid.Parse(registerOperationModel.Guid),
            ClientId = registerOperationModel.ClientId,
            CurrencyCode = registerOperationModel.Amount.CurrencyCode,
            Nanos = registerOperationModel.Amount.Nanos,
            Units = registerOperationModel.Amount.Units,
            OperationStatus = OperationStatusesEntity.Pending,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationTime = registerOperationModel.OperationTime
        };
    }

    public WithdrawOperationEntity MapOperationToWithdrawEntity(RegisterOperationModel registerOperationModel)
    {
        return new WithdrawOperationEntity()
        {
            Guid = Guid.Parse(registerOperationModel.Guid),
            ClientId = registerOperationModel.ClientId,
            CurrencyCode = registerOperationModel.Amount.CurrencyCode,
            Nanos = registerOperationModel.Amount.Nanos,
            Units = registerOperationModel.Amount.Units,
            OperationStatus = OperationStatusesEntity.Pending,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationTime = registerOperationModel.OperationTime
        };
    }
}