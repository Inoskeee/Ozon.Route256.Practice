namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;

internal sealed record FoodCategoryEntity
{
    public DateTime ProductionDateWithTime { get; init; }
    
    public int SelfLife { get; init; }
    
    public int SubCategory { get; init; }
}