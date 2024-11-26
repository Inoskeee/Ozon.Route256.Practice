using Ozon.Route256.Practice.ProductCardService.Domain.Entities;

namespace Ozon.Route256.Practice.ProductCardService.Application.Contracts;

public interface IProductService
{
    Task<ProductModel?> GetProduct(long skuId, CancellationToken cancellationToken);
    Task<bool> SetProduct(ProductModel productModel, CancellationToken cancellationToken);
    Task<bool> PublishProduct(long skuId, CancellationToken cancellationToken);
    Task<bool> UnpublishProduct(long skuId, CancellationToken cancellationToken);
}