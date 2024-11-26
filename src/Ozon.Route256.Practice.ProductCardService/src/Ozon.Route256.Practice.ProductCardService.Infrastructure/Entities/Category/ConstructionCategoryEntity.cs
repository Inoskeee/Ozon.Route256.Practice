namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;

internal sealed record ConstructionCategoryEntity
{
    public int Applicability { get; init; }
    
    public string Color { get; init; }
    
    public int SubCategory { get; init; }
}