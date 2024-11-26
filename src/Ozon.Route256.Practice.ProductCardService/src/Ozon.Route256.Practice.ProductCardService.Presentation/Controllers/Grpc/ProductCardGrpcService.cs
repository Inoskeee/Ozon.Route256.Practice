using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon.Route256.Practice.ProductCardService.Application.Contracts;
using Ozon.Route256.Practice.ProductCardService.Presentation.Contracts.Mappers;

namespace Ozon.Route256.Practice.ProductCardService.Presentation.Controllers.Grpc;

public class ProductCardGrpcService : ProductCardGrpc.ProductCardGrpcBase
{
    private readonly IProductService _productService;

    private readonly IProductMapper _productMapper;
    public ProductCardGrpcService(
        IProductService productService, 
        IProductMapper productMapper)
    {
        _productService = productService;
        _productMapper = productMapper;
    }

    public override async Task<V1GetSkuInfoResponse> V1GetSkuInfo(V1GetSkuInfoRequest request, ServerCallContext context)
    {
        var productModel = await _productService.GetProduct(request.SkuId.First(), context.CancellationToken);

        if (productModel is not null)
        {
            var productCard = _productMapper.MapModelToEntity(productModel);

            return new V1GetSkuInfoResponse()
            {
                ProductCards = { productCard }
            };
        }
        else
        {
            return new V1GetSkuInfoResponse();
        }
    }

    public override async Task<Empty> V1SetSkuInfo(V1SetSkuInfoRequest request, ServerCallContext context)
    {
        if (request.ProductCard.Category == Category.Food && request.ProductCard.CategoryAttributes.FoodAttributes is null
            || request.ProductCard.Category == Category.Clothes && request.ProductCard.CategoryAttributes.ClothesAttributes is null
            || request.ProductCard.Category == Category.Building && request.ProductCard.CategoryAttributes.BuildingAttributes is null)
        {
            throw new ArgumentException("The specified category does not match the product type.");
        }
        
        var productModel = _productMapper.MapEntityToModel(request.ProductCard);

        if (productModel is not null)
        {
            var isSuccess = await _productService.SetProduct(productModel, context.CancellationToken);
        }
        
        return new Empty();
    }

    public override async Task<Empty> V1PublishSku(V1PublishSkuRequest request, ServerCallContext context)
    {
        var isSuccess = await _productService.PublishProduct(request.SkuId, context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> V1UnpublishSku(V1UnpublishSkuRequest request, ServerCallContext context)
    {
        var isSuccess = await _productService.UnpublishProduct(request.SkuId, context.CancellationToken);
        
        return new Empty();
    }
}