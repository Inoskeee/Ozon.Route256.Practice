using MediatR;

using Ozon.Route256.CustomerService.Domain;

namespace Ozon.Route256.CustomerService.DomainServices.GetCustomers;

public sealed record GetCustomersQueryResponse(Customer[] Customers, long TotalCount)
{
}