using MediatR;
using Microsoft.Extensions.Logging;
using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Actions.DisableMismatchFeature;

public class DisableMismatchFeatureHandler : IRequestHandler<DisableMismatchFeatureCommand>
{
    private readonly IMismatchFeatureToggler _mismatchFeatureToggler;
    private readonly ILogger<DisableMismatchFeatureHandler> _logger;

    public DisableMismatchFeatureHandler(IMismatchFeatureToggler mismatchFeatureToggler, ILogger<DisableMismatchFeatureHandler> logger)
    {
        _mismatchFeatureToggler = mismatchFeatureToggler;
        _logger = logger;
    }

    public Task Handle(DisableMismatchFeatureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mismatch Feature Disabled");

        _mismatchFeatureToggler.Disable();

        return Task.CompletedTask;
    }
}
