namespace Ozon.Route256.Practice.ClientBalance.Bll.Models;

public sealed record MoneyModel(
    string CurrencyCode,
    long Units,
    int Nanos);