using Ozon.Route256.Practice.ClientOrders.Bll.Models.Customer;
using Ozon.Route256.Practice.CustomerService;

namespace Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Contracts.Mappers;

internal interface IRegionMapper
{
    RegionModel MapRegionDtoToModel(V1QueryCustomersResponse.Types.Region region);
}