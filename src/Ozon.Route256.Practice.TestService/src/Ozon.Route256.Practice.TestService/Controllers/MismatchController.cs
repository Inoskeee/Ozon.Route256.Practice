using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.TestService.Domain.Actions.ClearMismatches;
using Ozon.Route256.TestService.Domain.Actions.DisableMismatchFeature;
using Ozon.Route256.TestService.Domain.Actions.EnableMismatchFeature;
using Ozon.Route256.TestService.Domain.Actions.ListMismatches;
using Swashbuckle.AspNetCore.Annotations;

namespace Ozon.Route256.TestService.Controllers;

public class MismatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public MismatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(summary: "Получить статистику по выявленным несоответствиям.")]
    [ProducesResponseType(typeof(MismatchStatistics), StatusCodes.Status200OK)]
    [HttpGet("statistics")]
    public Task<MismatchStatistics> GetMismatchStatisticsAsync()
    {
        return _mediator.Send(new ListMismatchesQuery(), HttpContext.RequestAborted);
    }

    [SwaggerOperation(summary: "Очистить список несоответствий.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpDelete("clear")]
    public Task ClearAsync()
    {
        return _mediator.Send(new ClearMismatchesCommand(), HttpContext.RequestAborted);
    }

    [SwaggerOperation(summary: "Начать выявление несоответствий.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("start")]
    public Task StartAsync([FromBody] StartMismatchRequest request)
    {
        var command = new EnableMismatchFeatureCommand
        {
            Duration = request.Duration
        };

        return _mediator.Send(command, HttpContext.RequestAborted);
    }

    [SwaggerOperation(summary: "Остановить выявление несоответствий.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("stop")]
    public Task StopAsync()
    {
        return _mediator.Send(new DisableMismatchFeatureCommand(), HttpContext.RequestAborted);
    }
}
