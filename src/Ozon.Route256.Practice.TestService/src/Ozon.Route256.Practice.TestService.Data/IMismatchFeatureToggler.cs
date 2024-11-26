namespace Ozon.Route256.TestService.Data;

public interface IMismatchFeatureToggler
{
    void Enable(TimeSpan duration);

    void Disable();
}
