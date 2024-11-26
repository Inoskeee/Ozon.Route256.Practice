namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

internal sealed record ResultEntity<TResult>(
    bool IsSuccess, 
    TResult Result, 
    string Message);