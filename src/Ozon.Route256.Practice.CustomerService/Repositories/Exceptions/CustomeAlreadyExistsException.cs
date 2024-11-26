namespace Ozon.Route256.CustomerService.Repositories.Exceptions;

public sealed class CustomerAlreadyExistsException(string message) : Exception(message);