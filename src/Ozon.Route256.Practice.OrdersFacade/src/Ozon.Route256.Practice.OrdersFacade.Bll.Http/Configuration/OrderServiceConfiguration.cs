namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Configuration;

internal sealed class OrderServiceConfiguration
{
    public required string[] Urls { get; set; }
    public string Endpoint { get; set; } = null!;
}