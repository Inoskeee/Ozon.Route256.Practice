namespace Ozon.Route256.CustomerService.Domain;

public sealed class Customer
{
    public long Id { get; set; }
    public required string FullName { get; set; }
    public required Region Region { get; set; }
    public DateTime CreatedAt { get; set; }
}