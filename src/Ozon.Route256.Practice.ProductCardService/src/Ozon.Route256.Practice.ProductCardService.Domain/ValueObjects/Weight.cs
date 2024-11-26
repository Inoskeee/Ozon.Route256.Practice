namespace Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;

public class Weight
{
    public int Grams { get; }

    public Weight(int grams)
    {
        if (grams < 10)
        {
            throw new ArgumentException("The weight of the goods cannot be less than 10 grams.");
        }
        
        Grams = grams;
    }
}