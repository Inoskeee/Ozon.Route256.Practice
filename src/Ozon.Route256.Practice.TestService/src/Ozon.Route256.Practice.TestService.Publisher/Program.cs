using System.Text.Json;
using Confluent.Kafka;
using Google.Protobuf;
using Ozon.Route256.OrderService.Proto.Messages;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:29092,localhost:29093",
};

using var producer = new ProducerBuilder<string, byte[]>(config).Build();

var message = new OrderOutputEventMessage
{
    OrderId = 3,
    EventType = OutputEventType.Created
};

var result = await producer.ProduceAsync(
    "order_output_events",
    new Message<string, byte[]>
    {
        Key = message.OrderId.ToString(),
        Value = message.ToByteArray()
    });

Console.WriteLine(JsonSerializer.Serialize(result));
