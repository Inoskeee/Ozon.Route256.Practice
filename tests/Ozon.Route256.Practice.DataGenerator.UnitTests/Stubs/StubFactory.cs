namespace Ozon.Route256.DataGenerator.UnitTests.Stubs;

public static class StubFactory
{
    public static GenerateOrdersHandlerStub CreateGenerateOrdersHandlerStub()
        => new(new(), new(), new(), new(), new());
}
