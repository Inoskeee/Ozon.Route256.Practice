using Ozon.Route256.Practice.ProductCardService.Application.Contracts;
using Ozon.Route256.Practice.ProductCardService.Application.Contracts.External;
using Ozon.Route256.Practice.ProductCardService.Domain.Entities;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums;

namespace Ozon.Route256.Practice.ProductCardService.Application.Services;

public class ProductService : IProductService
{
    private readonly IRedisProvider _redisProvider;

    public ProductService(IRedisProvider redisProvider)
    {
        _redisProvider = redisProvider;
    }

    public async Task<ProductModel?> GetProduct(long skuId, CancellationToken cancellationToken)
    {
        var resultProduct = await _redisProvider.GetProduct(skuId, cancellationToken);
        
        return resultProduct;
    }

    public async Task<bool> SetProduct(ProductModel productModel, CancellationToken cancellationToken)
    {
        var resultSetProduct = await _redisProvider.InsertOrUpdateProduct(productModel, cancellationToken);
        
        return resultSetProduct;
    }

    public async Task<bool> PublishProduct(long skuId, CancellationToken cancellationToken)
    {
        var resultProduct = await _redisProvider.GetProduct(skuId, cancellationToken);

        if (resultProduct is not null && resultProduct.CommonAttributes.Status is CardStatus.Draft or CardStatus.Active)
        {
            resultProduct.CommonAttributes.UpdateStatus(CardStatus.Active);
            
            var resultPublishProduct = await _redisProvider.InsertOrUpdateProduct(resultProduct, cancellationToken);

            return resultPublishProduct;
        }
        
        return false;
    }

    public async Task<bool> UnpublishProduct(long skuId, CancellationToken cancellationToken)
    {
        var resultProduct = await _redisProvider.GetProduct(skuId, cancellationToken);

        if (resultProduct is not null && resultProduct.CommonAttributes.Status is CardStatus.Active)
        {
            resultProduct.CommonAttributes.UpdateStatus(CardStatus.Inactive);
            
            var resultUnpublishProduct = await _redisProvider.InsertOrUpdateProduct(resultProduct, cancellationToken);

            return resultUnpublishProduct;
        }
        
        return false;
    }
}