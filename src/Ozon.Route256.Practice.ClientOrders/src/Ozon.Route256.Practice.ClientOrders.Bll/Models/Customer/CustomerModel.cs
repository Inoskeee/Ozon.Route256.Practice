namespace Ozon.Route256.Practice.ClientOrders.Bll.Models.Customer;

public sealed record CustomerModel(
    long CustomerId, 
    RegionModel Region, 
    string FullName, 
    DateTimeOffset CreatedAt);