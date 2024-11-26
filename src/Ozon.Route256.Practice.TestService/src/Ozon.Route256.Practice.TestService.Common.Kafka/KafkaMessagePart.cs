namespace Ozon.Route256.TestService.Common.Kafka;

public class KafkaMessagePart<TPayload>
    where TPayload : class
{
    public byte[] RawValue { get; init; } = Array.Empty<byte>();

    public string Format { get; init; } = string.Empty;

    public TPayload? Payload { get; set; }

    public Exception? DeserializationException { get; set; }
}
