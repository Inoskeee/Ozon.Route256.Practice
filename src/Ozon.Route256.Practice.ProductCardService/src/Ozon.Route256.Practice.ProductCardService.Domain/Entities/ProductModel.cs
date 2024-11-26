using Ozon.Route256.Practice.ProductCardService.Domain.Enums;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities;

public sealed record ProductModel
{
    private CardCategory _cardCategory;
    
    public long SkuId { get; init; }

    public CommonAttributesModel? CommonAttributes { get; init; }
    
    public CategoryAttributesModel? CategoryAttributes { get; init; }

    public CardCategory CardCategory
    {
        get
        {
            return _cardCategory;
        }
        init
        {
            if (value == CardCategory.Undefined)
            {
                throw new ArgumentException("An invalid product category is specified.");
            }
            
            _cardCategory = value;
        }
    }
}