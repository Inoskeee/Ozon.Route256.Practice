using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Infrastructure;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderFacadeHttpService _orderFacadeHttpService;
    
    private readonly IHttpAggregateResponseMapper _responseMapper;
    
    private readonly IResponseValidatorProvider<OrdersByCustomerRequestDto, AggregateByCustomerResponseDto> _responseValidator;
    public OrdersController(
        IOrderFacadeHttpService orderFacadeHttpService, 
        IHttpAggregateResponseMapper responseMapper, 
        IResponseValidatorProvider<OrdersByCustomerRequestDto, AggregateByCustomerResponseDto> responseValidator)
    {
        _orderFacadeHttpService = orderFacadeHttpService;
        _responseMapper = responseMapper;
        _responseValidator = responseValidator;
    }

    [HttpPost("GetByCustomer")]
    [OrdersExceptionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AggregateByCustomerResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> GetOrdersByCustomer([FromBody] OrdersByCustomerRequestDto requestDto)
    {
        var aggregateResult =
            await _orderFacadeHttpService.GetOrderByCustomer(requestDto.CustomerId, requestDto.Limit, requestDto.Offset);
        
        var response = _responseMapper.MapByCustomerToResponse(aggregateResult);
        _responseValidator.Validate(requestDto, response);
        
        return Ok(response);
    }
    
    [HttpPost("GetByRegion")]
    [OrdersExceptionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AggregateByRegionResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> GetOrdersByClient([FromBody] OrdersByRegionRequestDto requestDto)
    {
        var aggregateResult =
            await _orderFacadeHttpService.GetOrderByRegion(requestDto.RegionId, requestDto.Limit, requestDto.Offset);
        
        var response = _responseMapper.MapByRegionToResponse(aggregateResult);
        
        return Ok(response);
    }
}