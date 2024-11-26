namespace Ozon.Route256.TestService.Data;

public class MismatchFeature : IMismatchFeature, IMismatchFeatureToggler
{
    private int _state;

    public bool IsEnabled { get; private set; }

    public void Enable(TimeSpan duration)
    {
        var state = Interlocked.Increment(ref _state);

        IsEnabled = true;

        _ = DisableAfterDelayAsync(state, duration);
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    private async Task DisableAfterDelayAsync(int state, TimeSpan duration)
    {
        await Task.Delay(duration);

        if (_state == state)
            Disable();
    }
}
