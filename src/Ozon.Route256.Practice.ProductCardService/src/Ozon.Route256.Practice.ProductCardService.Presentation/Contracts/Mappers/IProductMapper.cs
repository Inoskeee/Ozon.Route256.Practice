using Ozon.Route256.Practice.ProductCardService.Domain.Entities;

namespace Ozon.Route256.Practice.ProductCardService.Presentation.Contracts.Mappers;

public interface IProductMapper
{
    ProductModel? MapEntityToModel(ProductCard productCard);
    
    ProductCard? MapModelToEntity(ProductModel productModel);
}