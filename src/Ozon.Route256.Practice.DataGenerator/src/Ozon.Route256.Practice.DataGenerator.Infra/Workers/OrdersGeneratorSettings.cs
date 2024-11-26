namespace Ozon.Route256.DataGenerator.Infra.Workers;

public record OrdersGeneratorSettings
{
    public required int OrdersPerSecond { get; set; }

    public required int CustomersPerSecond { get; set; }

    public required int InvalidOrderCounterNumber { get; set; }
}
