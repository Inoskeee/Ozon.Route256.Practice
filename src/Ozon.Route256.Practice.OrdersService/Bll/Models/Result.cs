namespace Ozon.Route256.OrderService.Bll.Models;

public record Result(
    bool Success,
    ActionError? Error);