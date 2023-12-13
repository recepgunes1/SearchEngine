namespace UrlExtractor.Infrastructure.Extensions;

public static class Utils
{
    public static string ToAbsoluteUrl(this string relative, string baseUrl)
    {
        var uri = new Uri(new Uri(baseUrl), relative);
        return uri.AbsoluteUri;
    }
}