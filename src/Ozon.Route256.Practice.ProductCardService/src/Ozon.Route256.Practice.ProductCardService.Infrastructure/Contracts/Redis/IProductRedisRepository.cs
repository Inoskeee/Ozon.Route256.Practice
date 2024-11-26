using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;

internal interface IProductRedisRepository
{
    Task<bool> IsProductExists(long skuId, CancellationToken token);

    Task<ProductEntity?> GetProduct(long skuId, CancellationToken token);
    
    Task InsertProduct(ProductEntity product, CancellationToken token);
    
    Task UpdateProduct(ProductEntity product, CancellationToken token);
}