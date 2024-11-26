using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;

public interface IChangeOperationMapper
{
    TopUpOperationEntity MapOperationToTopUpEntity(ChangeOperationModel registerOperationModel);
    
    WithdrawOperationEntity MapOperationToWithdrawEntity(ChangeOperationModel registerOperationModel);
}