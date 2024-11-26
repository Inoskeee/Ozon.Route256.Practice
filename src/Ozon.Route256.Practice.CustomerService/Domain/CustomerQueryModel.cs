namespace Ozon.Route256.CustomerService.Domain;

public sealed class CustomerQueryModel
{
    public CustomerQueryModel(long toTalCount, Customer[] customers)
    {
        TotalCount = toTalCount;
        Customers = customers;
    }

    public long TotalCount { get; }
    public Customer[] Customers { get; }
}