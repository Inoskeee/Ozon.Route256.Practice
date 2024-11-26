using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ClothingEnums;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;

public record ClothingCategoryModel
{
    private ClothingSubCategory _subCategory;
    
    public ClothingSize InternationalSize { get; init; }
    
    public int NumericSize { get; init; }
    
    public string? Color { get; init; }
    
    public string? Material { get; init; }

    public ClothingSubCategory SubCategory
    {
        get
        {
            return _subCategory;
        }
        init
        {
            if (value == ClothingSubCategory.Undefined)
            {
                throw new ArgumentException("An invalid subcategory is specified");
            }
            else
            {
                _subCategory = value;
            }
        }
    }
}