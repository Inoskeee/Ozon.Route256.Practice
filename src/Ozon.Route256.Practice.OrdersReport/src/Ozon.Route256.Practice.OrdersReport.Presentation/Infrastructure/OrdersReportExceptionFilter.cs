using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ozon.Route256.Practice.OrdersReport.Presentation.Dtos;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Infrastructure;

public class OrdersReportExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException || context.Exception is RpcException
            {
                StatusCode: StatusCode.Cancelled
            })
        {
            var errorResponse = new ReportByCustomerResponseDto
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Формирование отчета было прервано новым запросом."
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status409Conflict
            };

            context.ExceptionHandled = true;
        }
        else
        {
            var errorResponse = new ReportByCustomerResponseDto
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = context.Exception.ToString()
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}