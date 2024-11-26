using Ozon.Route256.Practice.ClientBalance.Dal.Entities.Enums;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

public abstract class BaseOperationEntity()
{
    public Guid Guid { get; init; }
    
    public long ClientId { get; init; }
    
    public string CurrencyCode { get; init; } = null!;
    
    public long Units { get; init; }
    
    public int Nanos { get; init; }
    
    public OperationTypesEntity OperationTypeEntity { get; init; }
    
    public OperationStatusesEntity OperationStatus { get; init; }
    
    public DateTimeOffset OperationTime { get; init; }
}