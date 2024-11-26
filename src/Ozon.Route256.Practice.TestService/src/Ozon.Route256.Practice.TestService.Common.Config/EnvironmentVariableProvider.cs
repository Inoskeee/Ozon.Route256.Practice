namespace Ozon.Route256.TestService.Common.Config;

public class EnvironmentVariableProvider : IEnvironmentVariableProvider
{
    public string? GetValue(string key) => Environment.GetEnvironmentVariable(key);
}
