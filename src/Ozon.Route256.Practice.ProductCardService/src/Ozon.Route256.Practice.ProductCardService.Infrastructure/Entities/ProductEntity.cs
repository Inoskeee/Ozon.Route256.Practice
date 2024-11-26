namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;

internal sealed record ProductEntity
{
    public long SkuId { get; init; }
    
    public CommonAttributesEntity CommonAttributes { get; init; }
    
    public CategoryAttributesEntity CategoryAttributes { get; init; }

    public int CardCategory { get; init; }
}