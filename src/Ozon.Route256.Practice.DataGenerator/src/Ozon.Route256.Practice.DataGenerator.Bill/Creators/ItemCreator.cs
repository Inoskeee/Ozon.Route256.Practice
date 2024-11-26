using Bogus;
using Ozon.Route256.DataGenerator.Bll.Models;

namespace Ozon.Route256.DataGenerator.Bll.Creators;

public static class ItemCreator
{
    private static readonly Faker<Item> Faker = new Faker<Item>()
        .RuleFor(item => item.Barcode, faker => faker.Commerce.Ean13())
        .RuleFor(item => item.Quantity, faker => faker.Random.Int(1,20));

    private static readonly Faker<Item> InvalidItemFaker = new Faker<Item>()
        .RuleFor(item => item.Barcode, faker => faker.Commerce.Ean13())
        .RuleFor(item => item.Quantity, faker => faker.Random.Int(-20,-1));

    public static IReadOnlyList<Item> Create(int count = 1) => Faker.Generate(count);

    public static IReadOnlyList<Item> CreateInvalidItems(int count = 1) => InvalidItemFaker.Generate(count);
}
