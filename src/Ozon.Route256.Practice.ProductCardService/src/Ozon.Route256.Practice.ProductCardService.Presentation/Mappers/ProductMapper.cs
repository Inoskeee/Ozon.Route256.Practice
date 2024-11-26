using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.ProductCardService.Domain.Entities;
using Ozon.Route256.Practice.ProductCardService.Domain.Entities.Category;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ClothingEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.Enums.ConstructionEnums;
using Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;
using Ozon.Route256.Practice.ProductCardService.Presentation.Contracts.Mappers;
using Enum = System.Enum;

namespace Ozon.Route256.Practice.ProductCardService.Presentation.Mappers;

internal sealed class ProductMapper : IProductMapper
{
    public ProductModel? MapEntityToModel(ProductCard productCard)
    {
        var commonAttributes = new CommonAttributesModel()
        {
            ProductName = productCard.CommonAttributes.Name,
            PictureUrl = new PhotoUrl(productCard.CommonAttributes.PictureUrl),
            ProductionDate = DateOnly.FromDateTime(productCard.CommonAttributes.ProductionDate.ToDateTime()),
            Weight = new Weight(productCard.CommonAttributes.WeightGramm),
            Status = CardStatus.Draft
        };

        var foodCategory = MapFoodCategoryToModel(productCard.CategoryAttributes.FoodAttributes);
        var clothingCategory = MapClothingCategoryToModel(productCard.CategoryAttributes.ClothesAttributes);
        var constructionCategory = MapConstructionCategoryToModel(productCard.CategoryAttributes.BuildingAttributes);
        
        
        var categoryAttributes = new CategoryAttributesModel()
        {
            FoodCategory = foodCategory,
            ClothingCategory = clothingCategory,
            ConstructionCategory = constructionCategory
        };

        return new ProductModel()
        {
            SkuId = productCard.SkuId,
            CommonAttributes = commonAttributes,
            CategoryAttributes = categoryAttributes,
            CardCategory = (CardCategory)productCard.Category,
        };
    }

    public ProductCard? MapModelToEntity(ProductModel productModel)
    {
        var commonAttributes = new CommonAttributes()
        {
            Name = productModel.CommonAttributes?.ProductName,
            PictureUrl = productModel.CommonAttributes?.PictureUrl?.Value,
            ProductionDate = Timestamp.FromDateTime(productModel.CommonAttributes.ProductionDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)),
            WeightGramm = productModel.CommonAttributes.Weight.Grams
        };

        var foodCategory = MapFoodCategoryToEntity(productModel.CategoryAttributes.FoodCategory);
        var clothingCategory = MapClothingCategoryToEntity(productModel.CategoryAttributes.ClothingCategory);
        var constructionCategory = MapConstructionCategoryToEntity(productModel.CategoryAttributes.ConstructionCategory);


        var categoryAttributes = new CategoryAttributes();
        
        if (foodCategory is not null)
        {
            categoryAttributes.FoodAttributes = foodCategory;
        }
        else if (clothingCategory is not null)
        {
            categoryAttributes.ClothesAttributes = clothingCategory;
        }
        else if (constructionCategory is not null)
        {
            categoryAttributes.BuildingAttributes = constructionCategory;
        }
        
        return new ProductCard()
        {
            SkuId = productModel.SkuId,
            CommonAttributes = commonAttributes,
            CategoryAttributes = categoryAttributes,
            Category = (Category)productModel.CardCategory,
        };
    }
    
    private FoodCategoryModel? MapFoodCategoryToModel(FoodAttributes? foodAttributes)
    {
        if (foodAttributes is null)
        {
            return null;
        }
        
        return new FoodCategoryModel()
        {
            ProductionDateWithTime = foodAttributes.ProductionTime.ToDateTime(),
            SelfLife = new SelfLife(
                foodAttributes.ExpiresAfterHours, 
                (Domain.Enums.FoodEnums.FoodSubCategory)foodAttributes.SubCategory),
            SubCategory = (Domain.Enums.FoodEnums.FoodSubCategory)foodAttributes.SubCategory
        };
    }
    
    private FoodAttributes? MapFoodCategoryToEntity(FoodCategoryModel? foodCategoryModel)
    {
        if (foodCategoryModel is null)
        {
            return null;
        }
        
        return new FoodAttributes()
        {
            ExpiresAfterHours = foodCategoryModel.SelfLife.Hours,
            ProductionTime = foodCategoryModel.ProductionDateWithTime.ToTimestamp(),
            SubCategory = (FoodSubCategory)foodCategoryModel.SubCategory
        };
    }
    
    private ClothingCategoryModel? MapClothingCategoryToModel(ClothesAttributes? clothingCategory)
    {
        if (clothingCategory is null)
        {
            return null;
        }
        
        return new ClothingCategoryModel()
        {
            Material = clothingCategory.Material,
            InternationalSize = Enum.Parse<ClothingSize>(
                clothingCategory.InternationalSize, 
                ignoreCase: true),
            NumericSize = clothingCategory.RussiznSize,
            Color = clothingCategory.Color,
            SubCategory = (ClothingSubCategory)clothingCategory.SubCategory
        };
    }
    
    private ClothesAttributes? MapClothingCategoryToEntity(ClothingCategoryModel? clothingCategoryModel)
    {
        if (clothingCategoryModel is null)
        {
            return null;
        }
        
        return new ClothesAttributes()
        {
            Color = clothingCategoryModel.Color,
            InternationalSize = clothingCategoryModel.InternationalSize.ToString(),
            RussiznSize = clothingCategoryModel.NumericSize,
            Material = clothingCategoryModel.Material,
            SubCategory = (ClothesSubCategory)clothingCategoryModel.SubCategory
        };
    }
    
    private ConstructionCategoryModel? MapConstructionCategoryToModel(BuildingAttributes? buildingAttributes)
    {
        if (buildingAttributes is null)
        {
            return null;
        }
        
        return new ConstructionCategoryModel()
        {
            Applicability = (ConstructionApplicability)buildingAttributes.Applicability,
            Color = buildingAttributes.Color,
            SubCategory = (ConstructionSubCategory)buildingAttributes.SubCategory
        };
    }
    
    private BuildingAttributes? MapConstructionCategoryToEntity(ConstructionCategoryModel? constructionCategoryModel)
    {
        if (constructionCategoryModel is null)
        {
            return null;
        }
        
        return new BuildingAttributes()
        {
            Color = constructionCategoryModel.Color,
            Applicability = (BuildingApplicability)constructionCategoryModel.Applicability,
            SubCategory = (BuildingSubCategory)constructionCategoryModel.SubCategory
        };
    }
    
}