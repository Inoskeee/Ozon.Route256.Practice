using Ozon.Route256.Practice.ProductCardService.Domain.Enums.FoodEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;

public record FoodCategoryModel
{
    private FoodSubCategory _subCategory;
    public DateTime ProductionDateWithTime { get; init; }
    
    public SelfLife? SelfLife { get; init; }

    public FoodSubCategory SubCategory
    {
        get
        {
            return _subCategory;
        }
        init
        {
            if (value == FoodSubCategory.Undefined)
            {
                throw new ArgumentException("An invalid product subcategory is specified");
            }
            else
            {
                _subCategory = value;
            }
        }
    }
}