namespace Ozon.Route256.Practice.ClientOrders.Bll.Models.ResultModels;

public sealed record ActionErrorModel(
    string ErrorCode, 
    string ErrorMessage);