using AutoBogus;
using Bogus;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

public static class OrderEntityCreator
{
    private static readonly Faker<OrderEntity> Faker = new AutoFaker<OrderEntity>()
        .RuleFor(x => x.OrderId, f => f.Random.Long(10000))
        .RuleFor(x => x.RegionId, f => f.Random.Long(1))
        .RuleFor(x => x.CustomerId, f => f.Random.Long(10000))
        .RuleFor(x => x.Comment, f => f.Random.String(10, 'a', 'z'))
        .RuleFor(x => x.Status, f => f.Random.Int(1))
        .RuleFor(x => x.CreatedAt, f => f.Date.Past().ToUniversalTime());

    public static OrderEntity Generate() 
        => Faker.Generate();

    public static OrderEntity WithCreatedAt(
        this OrderEntity src,
        DateTime at)
        => src = src with { CreatedAt = at };
}