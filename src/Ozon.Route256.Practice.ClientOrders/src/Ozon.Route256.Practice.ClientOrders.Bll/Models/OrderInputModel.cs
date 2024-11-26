namespace Ozon.Route256.Practice.ClientOrders.Bll.Models;

public sealed record OrderInputModel(
    long RegionId,
    long CustomerId,
    string Comment,
    ItemModel[] Items);