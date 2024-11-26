using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Bll.Models.Enums;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;

public interface IRegisterOperationMapper
{
    TopUpOperationEntity MapOperationToTopUpEntity(RegisterOperationModel registerOperationModel);
    
    WithdrawOperationEntity MapOperationToWithdrawEntity(RegisterOperationModel registerOperationModel);
}