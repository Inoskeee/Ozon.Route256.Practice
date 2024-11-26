namespace Ozon.Route256.DataGenerator.Bll.Models;

public record Customer
{
    public long? Id { get; init; }

    public required string FullName { get; init; }

    public required long RegionId { get; init; }
}
