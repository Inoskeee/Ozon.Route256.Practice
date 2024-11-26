using MediatR;

namespace Ozon.Route256.CustomerService.DomainServices.GetCustomers;

public sealed record GetCustomersQueryRequest(long[] CustomerIds, long[] RegionIds, int Limit,  int Offset) : IRequest<GetCustomersQueryResponse>;