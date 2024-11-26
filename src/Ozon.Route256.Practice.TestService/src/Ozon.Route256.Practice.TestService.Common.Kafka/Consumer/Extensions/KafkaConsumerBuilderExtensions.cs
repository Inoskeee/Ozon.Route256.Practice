using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

public static class KafkaConsumerBuilderExtensions
{
    public static IKafkaConsumerBuilder<TKey, TValue> WithJsonValue<TKey, TValue>(this IKafkaConsumerBuilder<TKey, TValue> builder)
        where TKey : class
        where TValue : class
    {
        builder.Services.TryAddSingleton<IKafkaDeserializer<TValue>, KafkaJsonDeserializer<TValue>>();

        return builder;
    }

    public static IKafkaConsumerBuilder<TKey, TValue> WithProtoValue<TKey, TValue>(this IKafkaConsumerBuilder<TKey, TValue> builder)
        where TKey : class
        where TValue : class, IMessage<TValue>, new()
    {
        builder.Services.TryAddSingleton<IKafkaDeserializer<TValue>, KafkaProtoDeserializer<TValue>>();

        return builder;
    }

    public static IKafkaConsumerBuilder<TKey, TValue> WithStringKey<TKey, TValue>(this IKafkaConsumerBuilder<TKey, TValue> builder)
        where TKey : class
        where TValue : class
    {
        builder.Services.TryAddSingleton<IKafkaDeserializer<string>, KafkaUtf8StringDeserializer>();

        return builder;
    }
}
