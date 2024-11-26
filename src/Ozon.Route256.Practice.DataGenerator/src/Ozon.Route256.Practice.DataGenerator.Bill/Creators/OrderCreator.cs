using Bogus;
using Ozon.Route256.DataGenerator.Bll.Models;

namespace Ozon.Route256.DataGenerator.Bll.Creators;

public static class OrderCreator
{
    private static readonly Faker<Order> Faker = new Faker<Order>()
        .RuleFor(order => order.Comment, faker => faker.Random.Words(12));

    public static Order Create() => Faker.Generate();
}
