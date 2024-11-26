using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Actions.EnableMismatchFeature;

public class EnableMismatchFeatureHandler : IRequestHandler<EnableMismatchFeatureCommand>
{
    private readonly IOptionsSnapshot<MismatchOptions> _optionsSnapshot;
    private readonly IMismatchFeatureToggler _mismatchFeatureToggler;
    private readonly ILogger<EnableMismatchFeatureHandler> _logger;

    public EnableMismatchFeatureHandler(
        IOptionsSnapshot<MismatchOptions> optionsSnapshot,
        IMismatchFeatureToggler mismatchFeatureToggler,
        ILogger<EnableMismatchFeatureHandler> logger)
    {
        _optionsSnapshot = optionsSnapshot;
        _mismatchFeatureToggler = mismatchFeatureToggler;
        _logger = logger;
    }

    public TimeSpan SamplingDuration => _optionsSnapshot.Value.SamplingDuration;

    public Task Handle(EnableMismatchFeatureCommand request, CancellationToken cancellationToken)
    {
        var duration = request.Duration ?? SamplingDuration;

        _logger.LogInformation("Mismatch Feature Enabled for {Duration}", duration);

        _mismatchFeatureToggler.Enable(duration);

        return Task.CompletedTask;
    }
}
