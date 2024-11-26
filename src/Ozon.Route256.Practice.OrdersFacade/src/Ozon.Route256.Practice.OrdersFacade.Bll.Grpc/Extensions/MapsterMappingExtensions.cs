using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.CustomerService;
using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Extensions;

internal static class MapsterMappingExtensions
{
    public static void RegisterMapsterGrpcConfiguration(this IServiceCollection services)
    {
        services.RegisterGrpcOrdersMappingConfig();
        services.RegisterGrpcCustomerMappingConfig();
    }
    
    private static void RegisterGrpcOrdersMappingConfig(this IServiceCollection services)
    {
        TypeAdapterConfig<V1QueryOrdersResponse, OrderEntityWithCustomerId>.NewConfig()
            .Map(dest => dest.Status, src => (OrderStatusEnum)src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToDateTimeOffset());

        TypeAdapterConfig<BaseOrderEntity, GetOrderByCustomerResponse.Types.CustomerOrders>.NewConfig()
            .Map(dest => dest.Comment, src => new StringValue { Value = src.Comment })
            .Map(dest => dest.CreatedAt, src => Timestamp.FromDateTimeOffset(src.CreatedAt));
        
        TypeAdapterConfig<BaseOrderEntity, GetOrderByRegionResponse.Types.RegionOrders>.NewConfig()
            .Map(dest => dest.Comment, src => new StringValue { Value = src.Comment })
            .Map(dest => dest.CreatedAt, src => Timestamp.FromDateTimeOffset(src.CreatedAt));
    }

    private static void RegisterGrpcCustomerMappingConfig(this IServiceCollection services)
    {
        TypeAdapterConfig<V1QueryCustomersResponse.Types.Customer, CustomerEntity>.NewConfig()
            .Map(dest => dest.Region, src => src.Region.Adapt<RegionEntity>())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt.ToDateTimeOffset());
        
        TypeAdapterConfig<CustomerEntity, GetOrderByCustomerResponse.Types.Customer>.NewConfig()
            .Map(dest => dest.CreatedAt, src => Timestamp.FromDateTimeOffset(src.CreatedAt));
        
        TypeAdapterConfig<CustomerEntity, GetOrderByRegionResponse.Types.RegionOrders.Types.RegionCustomer>.NewConfig()
            .Map(dest => dest.CreatedAt, src => Timestamp.FromDateTimeOffset(src.CreatedAt));
    }
}