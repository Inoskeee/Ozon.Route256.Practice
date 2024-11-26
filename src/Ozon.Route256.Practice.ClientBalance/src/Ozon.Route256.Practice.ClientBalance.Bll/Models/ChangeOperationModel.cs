using Ozon.Route256.Practice.ClientBalance.Bll.Models.Enums;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Models;

public sealed record ChangeOperationModel(
    string Guid,
    long ClientId, 
    OperationTypesEnum OperationType, 
    OperationStatusesEnum OperationStatus,
    DateTimeOffset OperationTime);