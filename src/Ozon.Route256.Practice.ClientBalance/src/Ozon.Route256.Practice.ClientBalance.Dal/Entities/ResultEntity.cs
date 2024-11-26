namespace Ozon.Route256.Practice.ClientBalance.Dal.Entities;

public sealed record ResultEntity<TResult>(
    bool IsSuccess, 
    TResult Result, 
    string Message);