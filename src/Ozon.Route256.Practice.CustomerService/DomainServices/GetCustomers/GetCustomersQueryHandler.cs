using MediatR;

using Ozon.Route256.CustomerService.Repositories;

namespace Ozon.Route256.CustomerService.DomainServices.GetCustomers;

public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQueryRequest, GetCustomersQueryResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<GetCustomersQueryResponse> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _customerRepository.GetCustomers(request.CustomerIds, request.RegionIds, request.Limit, request.Offset, cancellationToken);
        return new GetCustomersQueryResponse(queryResult.Customers, queryResult.TotalCount);
    }
}