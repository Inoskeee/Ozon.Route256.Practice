using MediatR;

namespace Ozon.Route256.TestService.Domain.Actions.EnableMismatchFeature;

public class EnableMismatchFeatureCommand : IRequest
{
    public TimeSpan? Duration { get; set; }
}
