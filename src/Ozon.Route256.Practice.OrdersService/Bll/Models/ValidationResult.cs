using Ozon.Route256.OrderService.Kafka.Messages;

namespace Ozon.Route256.OrderService.Bll.Models;

public record ValidationResult(
    bool Success,
    OrderInputErrorsMessage Message);