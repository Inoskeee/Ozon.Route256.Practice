namespace Ozon.Route256.DataGenerator.Infra.Exceptions;

public class InvalidKafkaConfigurationException : Exception
{
    public InvalidKafkaConfigurationException(string message) : base(message) { }
}
