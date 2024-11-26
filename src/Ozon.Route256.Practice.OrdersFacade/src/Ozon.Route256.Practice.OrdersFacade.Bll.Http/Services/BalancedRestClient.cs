using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;
using RestSharp;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services;

/// <summary>
/// Кастомный балансировщик для Http запросов
/// Принцип работы: выбирает случайный адрес из массива, пытается выполнить запрос
/// Если запрос неудачный, переходит к другому адресу в массиве, пока не будут перебраны все адреса
/// </summary>
internal sealed class BalancedRestClient : IBalancedRestClient
{
    private readonly string[] _urls;
    private readonly RestClient _restClient;
    private int _currentServiceIndex;
    
    public BalancedRestClient(string[] urls)
    {
        _urls = urls;
        _restClient = new RestClient();

        Random random = new Random();
        _currentServiceIndex = random.Next(0, _urls.Length);
    }

    public async Task<RestResponse?> ExecuteAsync(RestRequest request, string endpoint)
    {
        RestResponse? response = null;
        bool success = false;
        for (int i = 0; i < _urls.Length && !success; i++)
        {
            var baseUrl = _urls[_currentServiceIndex];
            request.Resource = baseUrl + endpoint;

            try
            {
                response = await _restClient.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    success = true;
                }
                else
                {
                    _currentServiceIndex = (_currentServiceIndex + 1) % _urls.Length;
                }
            }
            catch (Exception)
            {
                _currentServiceIndex = (_currentServiceIndex + 1) % _urls.Length;
            }
        }

        return response;
    }
}