using MediatR;

using Ozon.Route256.CustomerService.Repositories;
using Ozon.Route256.CustomerService.Repositories.Exceptions;

namespace Ozon.Route256.CustomerService.DomainServices.CreateCustomer;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IRegionRepository _regionRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IRegionRepository regionRepository)
    {
        _customerRepository = customerRepository;
        _regionRepository = regionRepository;
    }

    public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var regionId = request.RegionId;
            var region = await _regionRepository.Get(regionId, cancellationToken);
            var customerId = await _customerRepository.CreateCustomer(request.FullName, region.Id, cancellationToken);
            return new CreateCustomerCommandResponse(customerId);
        }
        catch (Exception ex)
        {
            if (ex is RegionNotFoundException or CustomerAlreadyExistsException)
            {
                return new CreateCustomerCommandResponse(ex);
            }

            throw;
        }
    }
}