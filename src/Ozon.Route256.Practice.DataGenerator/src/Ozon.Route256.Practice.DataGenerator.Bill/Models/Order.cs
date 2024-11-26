namespace Ozon.Route256.DataGenerator.Bll.Models;

public record Order
{
    public required long RegionId { get; init; }

    public required long CustomerId { get; init; }

    public required string Comment { get; init; }

    public required IReadOnlyList<Item> Items { get; init; }
}
