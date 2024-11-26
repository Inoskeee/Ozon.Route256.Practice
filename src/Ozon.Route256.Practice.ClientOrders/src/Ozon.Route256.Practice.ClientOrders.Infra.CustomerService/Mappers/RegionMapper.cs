using Ozon.Route256.Practice.ClientOrders.Bll.Models.Customer;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Mappers;
using Ozon.Route256.Practice.CustomerService;

namespace Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Mappers;

internal sealed class RegionMapper : IRegionMapper
{
    public RegionModel MapRegionDtoToModel(V1QueryCustomersResponse.Types.Region region)
    {
        return new RegionModel(
            Id: region.Id,
            Name: region.Name);
    }
}