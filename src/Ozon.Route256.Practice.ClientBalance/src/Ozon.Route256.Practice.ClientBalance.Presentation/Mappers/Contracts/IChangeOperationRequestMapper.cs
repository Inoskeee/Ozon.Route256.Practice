using Ozon.Route256.Practice.ClientBalance.Bll.Models;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

public interface IChangeOperationRequestMapper
{
    ChangeOperationModel MapChangeOperationRequestToModel(ChangeOperationStatusRequest changeOperationStatusRequest);
}