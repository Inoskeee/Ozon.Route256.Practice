using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Infrastructure;

public class OrdersExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is CustomerNotFoundException)
        {
            var errorResponse = new ErrorResponseDto()
            {
                ErrorCode = (int)((OrderFacadeExceptionBase)context.Exception).ErrorCode,
                ErrorMessage = ((OrderFacadeExceptionBase)context.Exception).ErrorMessage
            };
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            
            context.ExceptionHandled = true;
        }
        else if(context.Exception is CustomerInternalServerException  || context.Exception is OrderInternalServerException)
        {
            var errorResponse = new ErrorResponseDto()
            {
                ErrorCode = (int)((OrderFacadeExceptionBase)context.Exception).ErrorCode,
                ErrorMessage = ((OrderFacadeExceptionBase)context.Exception).ErrorMessage
            };
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            
            context.ExceptionHandled = true;
        }
        else if(context.Exception is CustomerBadRequestException || context.Exception is OrderBadRequestException)
        {
            var errorResponse = new ErrorResponseDto()
            {
                ErrorCode = (int)((OrderFacadeExceptionBase)context.Exception).ErrorCode,
                ErrorMessage = ((OrderFacadeExceptionBase)context.Exception).ErrorMessage
            };
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            
            context.ExceptionHandled = true;
        }
        else
        {
            var errorResponse = new ErrorResponseDto()
            {
                ErrorCode = context.Exception.HResult,
                ErrorMessage = context.Exception.Message
            };
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            
            context.ExceptionHandled = true;
        }
    }
}