using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;

internal sealed record CategoryAttributesEntity
{
    public FoodCategoryEntity? FoodCategory { get; init; }
    
    public ClothingCategoryEntity? ClothingCategory { get; init; }
    
    public ConstructionCategoryEntity? ConstructionCategory { get; init; }
}