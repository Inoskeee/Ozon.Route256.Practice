using System.Text.Json;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Redis;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;
using StackExchange.Redis;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Redis.Repositories;

internal class ProductRedisRepository : IProductRedisRepository
{
    private readonly IDatabase _redisDatabase;
    private readonly IServer _redisServer;
    
    public ProductRedisRepository(IRedisDatabaseFactory redisDatabaseFactory)
    {
        _redisDatabase = redisDatabaseFactory.GetDatabase();
        _redisServer = redisDatabaseFactory.GetServer();
    }

    public async Task<bool> IsProductExists(long skuId, CancellationToken token)
    {
        var result = await _redisDatabase.KeyExistsAsync(GetKey(skuId)).WaitAsync(token);
        return result;
    }

    public async Task<ProductEntity?> GetProduct(long skuId, CancellationToken token)
    {
        var productValue = await _redisDatabase.StringGetAsync(GetKey(skuId)).WaitAsync(token);

        if (productValue.IsNullOrEmpty)
        {
            return null;
        }
        
        string productJson = productValue.ToString();

        return JsonSerializer.Deserialize<ProductEntity>(productJson);
    }

    public async Task InsertProduct(ProductEntity product, CancellationToken token)
    {
        var redisValue = JsonSerializer.Serialize(product, product.GetType());
        
        await _redisDatabase.StringSetAsync(
                GetKey(skuId: product.SkuId),
                redisValue,
                when: When.NotExists
            )
            .WaitAsync(token);
    }

    public async Task UpdateProduct(ProductEntity product, CancellationToken token)
    {
        var redisValue = JsonSerializer.Serialize(product, product.GetType());

        await _redisDatabase.StringSetAsync(
                GetKey(skuId: product.SkuId),
                redisValue,
                when: When.Exists
            )
            .WaitAsync(token);
    }

    private string GetKey(long skuId)
    {
        return skuId.ToString();
    }
}