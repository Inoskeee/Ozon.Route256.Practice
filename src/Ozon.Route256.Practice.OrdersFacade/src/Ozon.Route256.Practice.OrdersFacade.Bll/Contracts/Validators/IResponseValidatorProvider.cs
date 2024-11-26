namespace Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;

public interface IResponseValidatorProvider<in TRequest, in TResponse>
{
    void Validate(TRequest request, TResponse response);
}