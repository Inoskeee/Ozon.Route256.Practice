namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;

internal sealed record CommonAttributesEntity
{
    public string ProductName { get; init; }
    
    public DateOnly ProductionDate { get; init; }
    
    public int Weight { get; init; }
    
    public string PictureUrl { get; init; }
    
    public int Status { get; init; }
}