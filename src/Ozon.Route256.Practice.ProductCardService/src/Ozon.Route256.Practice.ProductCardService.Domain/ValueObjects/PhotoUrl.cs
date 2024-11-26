using System.Text.RegularExpressions;

namespace Ozon.Route256.Practice.ProductCardService.Domain.ValueObjects;

public class PhotoUrl
{
    private static readonly Regex UrlPattern = new Regex(
        @"^(https?://)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}(/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$", 
        RegexOptions.Compiled);

    public string Value { get; }

    public PhotoUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product photo URL cannot be empty or null.");
        }

        if (!IsValidUrl(value))
        {
            throw new ArgumentException("Invalid URL format.");
        }

        Value = value;
    }

    private static bool IsValidUrl(string url)
    {
        return UrlPattern.IsMatch(url);
    }
}