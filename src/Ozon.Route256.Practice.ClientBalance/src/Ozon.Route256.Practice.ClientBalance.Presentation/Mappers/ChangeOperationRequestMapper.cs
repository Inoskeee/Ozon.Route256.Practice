using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Bll.Models.Enums;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers;

public class ChangeOperationRequestMapper : IChangeOperationRequestMapper
{
    public ChangeOperationModel MapChangeOperationRequestToModel(ChangeOperationStatusRequest changeOperationStatusRequest)
    {
        return new ChangeOperationModel(
            changeOperationStatusRequest.Guid,
            changeOperationStatusRequest.ClientId,
            (OperationTypesEnum)changeOperationStatusRequest.OperationType,
            (OperationStatusesEnum)changeOperationStatusRequest.OperationStatus,
            changeOperationStatusRequest.OperationTime.ToDateTimeOffset());
    }
}