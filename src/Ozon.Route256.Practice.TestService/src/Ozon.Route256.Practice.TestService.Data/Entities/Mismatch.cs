namespace Ozon.Route256.TestService.Data;

public class Mismatch
{
    public MismatchType Type { get; set; }

    public required string Key { get; init; }

    public required object Payload { get; init; }

    public required object StoredData { get; init; }

    public DateTimeOffset CreateAt { get; } = DateTimeOffset.UtcNow;
}
