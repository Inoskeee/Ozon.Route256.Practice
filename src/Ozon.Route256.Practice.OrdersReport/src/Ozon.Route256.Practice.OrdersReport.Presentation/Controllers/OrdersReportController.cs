using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Helpers;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;
using Ozon.Route256.Practice.OrdersReport.Bll.Services;
using Ozon.Route256.Practice.OrdersReport.Presentation.Dtos;
using Ozon.Route256.Practice.OrdersReport.Presentation.Infrastructure;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Controllers;

public class OrdersReportController(
    ILogger<OrdersReportController> logger,
    IOrdersReportService ordersReportService,
    IOrdersReportStateProvider stateProvider)
    : ControllerBase
{
    [HttpPost("GetReportByCustomer")]
    [OrdersReportExceptionFilter]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ReportByCustomerResponseDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ReportByCustomerResponseDto))]
    public async Task<IActionResult> GetOrdersReportByCustomer([FromBody] ReportByCustomerRequestDto reportRequest,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Поступил запрос на получение отчета: {request}",
            JsonSerializer.Serialize(reportRequest));

        if (stateProvider.ActiveRequests.TryGetValue(reportRequest.CustomerId, out var existingCts))
        {
            logger.LogInformation("Отмена предыдущего активного запроса для клиента {customerId}",
                reportRequest.CustomerId);
            existingCts?.Cancel();
        }

        var currentCancellationToken = new CancellationTokenSource();
        stateProvider.ActiveRequests[reportRequest.CustomerId] = currentCancellationToken;
        cancellationToken.Register(() => currentCancellationToken.Cancel());

        try
        {
            await stateProvider.RateLimitSemaphore.WaitAsync(currentCancellationToken.Token);

            var formatter = new CsvReportFormatter();
            var resultCvs =
                await ordersReportService.GetReportByCustomer(reportRequest.CustomerId, formatter,
                    currentCancellationToken.Token);

            logger.LogInformation("Запрос обработан. Генерация отчета для клиента {customerId} завершена.",
                reportRequest.CustomerId);

            return File(
                Encoding.UTF8.GetBytes(resultCvs.ToString()),
                "text/csv",
                $"OrderReport_{reportRequest.CustomerId}.csv");
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Запрос отменен для клиента {customerId}", reportRequest.CustomerId);
            return Conflict(new ReportByCustomerResponseDto
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = "Формирование отчета было прервано новым запросом."
            });
        }
        finally
        {
            stateProvider.RateLimitSemaphore.Release();
            if (stateProvider.ActiveRequests.TryRemove(reportRequest.CustomerId, out var activeCts))
                activeCts?.Dispose();
        }
    }
}