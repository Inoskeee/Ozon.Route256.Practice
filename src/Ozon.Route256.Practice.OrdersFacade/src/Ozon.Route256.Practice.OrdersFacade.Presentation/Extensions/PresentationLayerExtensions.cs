using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Validators;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Extensions;

public static class PresentationLayerExtensions
{
    public static IServiceCollection ConfigurePresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Ozon.Route256.Practice.OrdersFacade", Version = "v1" });
        });

        services
            .AddScoped<IResponseValidatorProvider<OrdersByCustomerRequestDto, AggregateByCustomerResponseDto>,
                ResponseOrderByCustomerValidator>();
        services.AddScoped<IHttpAggregateResponseMapper, HttpAggregateResponseMapper>();
        
        services.AddValidatorsFromAssemblyContaining<RequestOrdersByCustomerValidator>();
        services.AddValidatorsFromAssemblyContaining<RequestOrdersByRegionValidator>();
        
        return services;
    }

}