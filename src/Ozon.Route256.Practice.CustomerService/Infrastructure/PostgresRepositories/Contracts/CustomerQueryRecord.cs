namespace Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories.Contracts;

public sealed class CustomerQueryRecord
{
    public long CustomerId { get; set; }
    public required string FullName { get; set; }
    public long RegionId { get; set; }
    public string? RegionName { get; set; }
    public DateTime CreatedAt { get; set; }
    public long TotalCount { get; set; }
}