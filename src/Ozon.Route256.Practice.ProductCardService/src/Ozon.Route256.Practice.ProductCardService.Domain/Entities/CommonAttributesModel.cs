using Ozon.Route256.Practice.ProductCardService.Domain.Enums;
using Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;

namespace Ozon.Route256.Practice.ProductCardService.Domain.Entities;

public sealed record CommonAttributesModel
{
    private CardStatus _cardStatus = CardStatus.Draft;
    public string? ProductName { get; init; }
    
    public DateOnly ProductionDate { get; init; }
    
    public Weight? Weight { get; init; }
    
    public PhotoUrl? PictureUrl { get; init; }

    public CardStatus Status
    {
        get
        {
            return _cardStatus;
        }
        init
        {
            if (value == CardStatus.Undefined)
            {
                throw new ArgumentException("An invalid product status is specified");
            }
            else
            {
                _cardStatus = value;
            }
        }
    }

    public void UpdateStatus(CardStatus newStatus)
    {
        _cardStatus = newStatus;
    }
}