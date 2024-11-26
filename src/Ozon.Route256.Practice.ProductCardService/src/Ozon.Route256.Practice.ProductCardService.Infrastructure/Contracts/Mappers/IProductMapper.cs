using Ozon.Route256.Practice.ProductCardService.Domain.Entities;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Mappers;

internal interface IProductMapper
{
    ProductModel? MapEntityToModel(ProductEntity productEntity);
    
    ProductEntity? MapModelToEntity(ProductModel productModel);
}