using Bogus;

namespace Ozon.Route256.DataGenerator.Bll.Creators;

public static class Random
{
    private static readonly Faker Faker = new();

    private static long _counter = DateTime.UtcNow.Ticks;

    public static long RandomId => Interlocked.Increment(ref _counter);

    public static int InvalidRegion => Faker.Random.Int(10, 10000);

    public static int ItemsCount => Faker.Random.Int(1, 10);

    public static T EnumValue<T>(params T[] exclude)
        where T : struct, Enum
        => Faker.Random.Enum(exclude);

    public static T Element<T>(IReadOnlyList<T> list) => Faker.PickRandom(list.ToArray());
}
