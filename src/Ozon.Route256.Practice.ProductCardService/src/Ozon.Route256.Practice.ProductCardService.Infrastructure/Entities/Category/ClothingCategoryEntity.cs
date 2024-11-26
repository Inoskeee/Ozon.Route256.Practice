namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;

internal sealed record ClothingCategoryEntity
{
    public int InternationalSize { get; init; }
    
    public int NumericSize { get; init; }
    
    public string Color { get; init; }
    
    public string Material { get; init; }
    
    public int SubCategory { get; init; }
}