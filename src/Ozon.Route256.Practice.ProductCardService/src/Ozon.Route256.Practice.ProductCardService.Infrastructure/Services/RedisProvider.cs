using Ozon.Route256.Practice.ProductCardService.Application.Contracts.External;
using Ozon.Route256.Practice.ProductCardService.Domain.Entities;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Mappers;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Services;

internal sealed class RedisProvider : IRedisProvider
{
    private readonly IProductRedisRepository _redisRepository;
    private readonly IProductMapper _productMapper;

    public RedisProvider(
        IProductRedisRepository redisRepository, 
        IProductMapper productMapper)
    {
        _redisRepository = redisRepository;
        _productMapper = productMapper;
    }

    public async Task<ProductModel?> GetProduct(long skuId, CancellationToken cancellationToken)
    {
        var isProductExists = await _redisRepository.IsProductExists(skuId, cancellationToken);

        if (isProductExists)
        {
            var resultProduct = await _redisRepository.GetProduct(skuId, cancellationToken);

            if (resultProduct is not null)
            {
                var productEntity = _productMapper.MapEntityToModel(resultProduct);
                return productEntity;
            }
        }

        return null;
    }

    public async Task<bool> InsertOrUpdateProduct(ProductModel productModel, CancellationToken cancellationToken)
    {
        var isProductExists = await _redisRepository.IsProductExists(productModel.SkuId, cancellationToken);
        var resultProduct = await _redisRepository.GetProduct(productModel.SkuId, cancellationToken);
        
        var productEntity = _productMapper.MapModelToEntity(productModel);
        
        if (!isProductExists 
            && productEntity is not null)
        {
            await _redisRepository.InsertProduct(productEntity, cancellationToken);
            return true;
        }
        else if(productEntity is not null 
                && resultProduct is not null 
                && productEntity.CardCategory == resultProduct.CardCategory)
        {
            await _redisRepository.UpdateProduct(productEntity, cancellationToken);
            return true;
        }
        
        return false;
    }
}