using Ozon.Route256.Practice.ProductCardService.Domain.Entities;
using Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ClothingEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ConstructionEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums.FoodEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Contracts.Mappers;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Entities.Category;

namespace Ozon.Route256.Practice.ProductCardService.Infrastructure.Mappers;

internal sealed class ProductMapper : IProductMapper
{
    public ProductModel? MapEntityToModel(ProductEntity productEntity)
    {
        var commonAttributes = new CommonAttributesModel()
        {
            ProductName = productEntity.CommonAttributes.ProductName,
            PictureUrl = new PhotoUrl(productEntity.CommonAttributes.PictureUrl),
            ProductionDate = productEntity.CommonAttributes.ProductionDate,
            Weight = new Weight(productEntity.CommonAttributes.Weight),
            Status = (CardStatus)productEntity.CommonAttributes.Status
        };

        var foodCategory = MapFoodCategoryToModel(productEntity.CategoryAttributes.FoodCategory);
        var clothingCategory = MapClothingCategoryToModel(productEntity.CategoryAttributes.ClothingCategory);
        var constructionCategory = MapConstructionCategoryToModel(productEntity.CategoryAttributes.ConstructionCategory);
        
        
        var categoryAttributes = new CategoryAttributesModel()
        {
            FoodCategory = foodCategory,
            ClothingCategory = clothingCategory,
            ConstructionCategory = constructionCategory
        };

        return new ProductModel()
        {
            SkuId = productEntity.SkuId,
            CardCategory = (CardCategory)productEntity.CardCategory,
            CommonAttributes = commonAttributes,
            CategoryAttributes = categoryAttributes
        };
    }

    public ProductEntity? MapModelToEntity(ProductModel productModel)
    {
        var commonAttributes = new CommonAttributesEntity()
        {
            ProductName = productModel.CommonAttributes.ProductName,
            PictureUrl = productModel.CommonAttributes.PictureUrl.Value,
            ProductionDate = productModel.CommonAttributes.ProductionDate,
            Weight = productModel.CommonAttributes.Weight.Grams,
            Status = (int)productModel.CommonAttributes.Status
        };

        var foodCategory = MapFoodCategoryToEntity(productModel.CategoryAttributes.FoodCategory);
        var clothingCategory = MapClothingCategoryToEntity(productModel.CategoryAttributes.ClothingCategory);
        var constructionCategory = MapConstructionCategoryToEntity(productModel.CategoryAttributes.ConstructionCategory);
        
        
        var categoryAttributes = new CategoryAttributesEntity()
        {
            FoodCategory = foodCategory,
            ClothingCategory = clothingCategory,
            ConstructionCategory = constructionCategory
        };

        return new ProductEntity()
        {
            SkuId = productModel.SkuId,
            CardCategory = (int)productModel.CardCategory,
            CommonAttributes = commonAttributes,
            CategoryAttributes = categoryAttributes
        };
    }
    
    private FoodCategoryModel? MapFoodCategoryToModel(FoodCategoryEntity? foodCategoryEntity)
    {
        if (foodCategoryEntity is null)
        {
            return null;
        }
        
        return new FoodCategoryModel()
        {
            ProductionDateWithTime = foodCategoryEntity.ProductionDateWithTime,
            SelfLife = new SelfLife(foodCategoryEntity.SelfLife, (FoodSubCategory)foodCategoryEntity.SubCategory),
            SubCategory = (FoodSubCategory)foodCategoryEntity.SubCategory
        };
    }
    
    private FoodCategoryEntity? MapFoodCategoryToEntity(FoodCategoryModel? foodCategoryModel)
    {
        if (foodCategoryModel is null)
        {
            return null;
        }
        
        return new FoodCategoryEntity()
        {
            SelfLife = foodCategoryModel.SelfLife.Hours,
            ProductionDateWithTime = foodCategoryModel.ProductionDateWithTime,
            SubCategory = (int)foodCategoryModel.SubCategory
        };
    }
    
    private ClothingCategoryModel? MapClothingCategoryToModel(ClothingCategoryEntity? clothingCategoryEntity)
    {
        if (clothingCategoryEntity is null)
        {
            return null;
        }
        
        return new ClothingCategoryModel()
        {
            Material = clothingCategoryEntity.Material,
            InternationalSize = (ClothingSize)clothingCategoryEntity.InternationalSize,
            NumericSize = clothingCategoryEntity.NumericSize,
            Color = clothingCategoryEntity.Color,
            SubCategory = (ClothingSubCategory)clothingCategoryEntity.SubCategory
        };
    }
    
    private ClothingCategoryEntity? MapClothingCategoryToEntity(ClothingCategoryModel? clothingCategoryModel)
    {
        if (clothingCategoryModel is null)
        {
            return null;
        }
        
        return new ClothingCategoryEntity()
        {
            Color = clothingCategoryModel.Color,
            InternationalSize = (int)clothingCategoryModel.InternationalSize,
            NumericSize = clothingCategoryModel.NumericSize,
            Material = clothingCategoryModel.Material,
            SubCategory = (int)clothingCategoryModel.SubCategory
        };
    }
    
    private ConstructionCategoryModel? MapConstructionCategoryToModel(ConstructionCategoryEntity? constructionCategoryEntity)
    {
        if (constructionCategoryEntity is null)
        {
            return null;
        }
        
        return new ConstructionCategoryModel()
        {
            Applicability = (ConstructionApplicability)constructionCategoryEntity.Applicability,
            Color = constructionCategoryEntity.Color,
            SubCategory = (ConstructionSubCategory)constructionCategoryEntity.SubCategory
        };
    }
    
    private ConstructionCategoryEntity? MapConstructionCategoryToEntity(ConstructionCategoryModel? constructionCategoryModel)
    {
        if (constructionCategoryModel is null)
        {
            return null;
        }
        
        return new ConstructionCategoryEntity()
        {
            Color = constructionCategoryModel.Color,
            Applicability = (int)constructionCategoryModel.Applicability,
            SubCategory = (int)constructionCategoryModel.SubCategory
        };
    }
}