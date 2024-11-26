using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Customer;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Services;

namespace Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Services;

internal sealed class CustomerServiceProvider : ICustomerServiceProvider
{
    private readonly ICustomerGrpcClientProvider _clientProvider;

    private readonly IRegionMapper _regionMapper;
    
    public CustomerServiceProvider(
        ICustomerGrpcClientProvider clientProvider, 
        IRegionMapper regionMapper)
    {
        _clientProvider = clientProvider;
        _regionMapper = regionMapper;
    }

    public async Task<RegionModel> GetCustomerRegion(long customerId)
    {
        var customerResponse = await _clientProvider.GetCustomersAsync(
            customerId: new[] {customerId}, 
            limit: 1, 
            offset: 0);

        var regionModel = _regionMapper.MapRegionDtoToModel(customerResponse.First().Customers.First().Region);

        return regionModel;
    }
}