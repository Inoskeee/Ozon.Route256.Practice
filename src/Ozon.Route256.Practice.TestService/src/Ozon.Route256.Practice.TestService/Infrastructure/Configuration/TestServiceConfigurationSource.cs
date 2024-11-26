namespace Ozon.Route256.TestService;

public class TestServiceConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => new TestServiceEnvironmentConfigurationProvider();
}
