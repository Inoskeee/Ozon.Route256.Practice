namespace Ozon.Route256.Practice.ClientBalance.Bll.Models;

public sealed record RegisterOperationModel(
    string Guid,
    long ClientId, 
    MoneyModel Amount,
    DateTimeOffset OperationTime);