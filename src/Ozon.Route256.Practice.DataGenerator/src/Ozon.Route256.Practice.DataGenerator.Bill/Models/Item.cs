namespace Ozon.Route256.DataGenerator.Bll.Models;

public record Item
{
    public required string Barcode { get; init; }

    public required int Quantity { get; init; }
}
