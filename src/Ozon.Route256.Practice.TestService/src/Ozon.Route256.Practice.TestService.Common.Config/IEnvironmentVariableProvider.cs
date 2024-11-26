namespace Ozon.Route256.TestService.Common.Config;

public interface IEnvironmentVariableProvider
{
    string? GetValue(string key);
}
