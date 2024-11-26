using Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities;

public sealed record CategoryAttributesModel
{
    public FoodCategoryModel? FoodCategory { get; init; }
    
    public ClothingCategoryModel? ClothingCategory { get; init; }
    
    public ConstructionCategoryModel? ConstructionCategory { get; init; }
}