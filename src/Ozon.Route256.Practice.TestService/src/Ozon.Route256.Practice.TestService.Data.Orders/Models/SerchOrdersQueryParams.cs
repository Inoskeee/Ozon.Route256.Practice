namespace Ozon.Route256.TestService.Data.Orders;

public class SearchOrdersQueryParams
{
    public required long RegionId { get; init; }

    public required long CustomerId { get; init; }

    public required IReadOnlyCollection<SearchOrdersItem> Items { get; init; }

    public string[] GetItemsBarcode() => Items
        .Select(item => item.Barcode)
        .ToArray();

    public int[] GetItemsQuantity() => Items
        .Select(item => item.Quantity)
        .ToArray();

    public class SearchOrdersItem
    {
        public required string Barcode { get; set; }

        public required int Quantity { get; set; }
    }
}
