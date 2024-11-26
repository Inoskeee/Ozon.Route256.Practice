namespace Ozon.Route256.Practice.ClientBalance.Dal.Entities;

public sealed record ClientBalanceEntity(
    long ClientId,
    string CurrencyCode,
    long Units,
    int Nanos);