using MediatR;

namespace Ozon.Route256.CustomerService.DomainServices.CreateCustomer;

public sealed record CreateCustomerCommandRequest(string FullName, long RegionId) : IRequest<CreateCustomerCommandResponse>;