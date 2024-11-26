using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ConstructionEnums;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;

public record ConstructionCategoryModel
{
    public ConstructionApplicability Applicability { get; init; }
    
    public string? Color { get; init; }
    
    public ConstructionSubCategory SubCategory { get; init; }
}