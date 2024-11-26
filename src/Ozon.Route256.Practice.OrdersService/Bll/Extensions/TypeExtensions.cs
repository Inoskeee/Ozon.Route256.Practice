namespace Ozon.Route256.OrderService.Bll.Extensions;

public static class TypeExtensions
{
    public static T[] MakeArray<T>(this T obj)
    {
        return obj is not null ? new[] { obj } : Array.Empty<T>();
    }
}