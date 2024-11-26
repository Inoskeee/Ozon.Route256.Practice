using System.Text.Json.Serialization;

namespace Ozon.Route256.TestService.Data;

[JsonConverter(typeof(JsonStringEnumConverter<MismatchType>))]
public enum MismatchType
{
    None = 0,
    OrderNotExist = 1,
    OrderCreatedOnError = 2,
    CustomerNotExist = 3
}
