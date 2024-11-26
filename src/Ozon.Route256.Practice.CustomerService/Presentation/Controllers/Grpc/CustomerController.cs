using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using MediatR;

using Ozon.Route256.CustomerService.DomainServices.CreateCustomer;
using Ozon.Route256.CustomerService.DomainServices.GetCustomers;

namespace Ozon.Route256.CustomerService.Presentation.Controllers.Grpc;

public sealed class CustomerController : CustomerService.CustomerServiceBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator  mediator)
    {
        _mediator = mediator;
    }

    public override async Task<V1CreateCustomerResponse> V1CreateCustomer(V1CreateCustomerRequest request, ServerCallContext context)
    {
        var internalRequest = new CreateCustomerCommandRequest(request.FullName, request.RegionId);
        var internalResponse = await _mediator.Send(internalRequest, context.CancellationToken);
        return internalResponse.Successful
            ? new V1CreateCustomerResponse { Ok = new V1CreateCustomerResponse.Types.Success { CustomerId = internalResponse.CustomerId!.Value } }
            : new V1CreateCustomerResponse { Error = new V1CreateCustomerResponse.Types.Error { Code = internalResponse.Exception!.GetType().Name,  Text = internalResponse.Exception.Message } };
    }

    public override async Task V1QueryCustomers(V1QueryCustomersRequest request, IServerStreamWriter<V1QueryCustomersResponse> responseStream, ServerCallContext context)
    {
        var internalRequest = new GetCustomersQueryRequest(request.CustomerIds.ToArray(), request.RegionIds.ToArray(), request.Limit, request.Offset);
        var internalResponse = await _mediator.Send(internalRequest, context.CancellationToken);
        var response = new V1QueryCustomersResponse { TotalCount = internalResponse.TotalCount };
        var customers = internalResponse.Customers.Select(
            c => new V1QueryCustomersResponse.Types.Customer
            {
                CustomerId = c.Id,
                FullName = c.FullName,
                Region = new V1QueryCustomersResponse.Types.Region { Id = c.Region.Id, Name = c.Region.Name },
                CreatedAt = c.CreatedAt.ToTimestamp()
            });
        response.Customers.AddRange(customers);
        await responseStream.WriteAsync(response);
    }
}
