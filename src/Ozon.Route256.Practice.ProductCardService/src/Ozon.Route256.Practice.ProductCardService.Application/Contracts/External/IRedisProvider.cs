using Ozon.Route256.Practice.ProductCardService.Domain.Entities;

namespace Ozon.Route256.Practice.ProductCardService.Application.Contracts.External;

public interface IRedisProvider
{
    Task<ProductModel?> GetProduct(long skuId, CancellationToken cancellationToken);
    Task<bool> InsertOrUpdateProduct(ProductModel productModel, CancellationToken cancellationToken);
}