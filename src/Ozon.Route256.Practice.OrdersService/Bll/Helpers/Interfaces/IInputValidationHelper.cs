using Ozon.Route256.OrderService.Bll.Models;

namespace Ozon.Route256.OrderService.Bll.Helpers.Interfaces;

public interface IInputValidationHelper
{
    Task<ValidationResult> ValidateInputOrder(
        InputOrder inputOrder,
        CancellationToken token);
}