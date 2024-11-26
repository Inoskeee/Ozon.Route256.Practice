using Ozon.Route256.Practice.ProductCardService.Domain.Enums.FoodEnums;

namespace Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;

public class SelfLife
{
    public int Hours { get; }

    public SelfLife(int hours, FoodSubCategory category)
    {
        if (hours <= 0)
        {
            throw new ArgumentException("Self Life must be a positive number of hours.");
        }

        if (category != FoodSubCategory.Grocery && hours > 744)
        {
            throw new ArgumentException("The shelf life of the product exceeds one month.");
        }
        
        Hours = hours;
    }
}