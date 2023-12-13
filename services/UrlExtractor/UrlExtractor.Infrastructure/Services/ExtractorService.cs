using HtmlAgilityPack;
using UrlExtractor.Infrastructure.Extensions;

namespace UrlExtractor.Infrastructure.Services;

public class ExtractorService : IExtractorService
{
    public IEnumerable<string> ExtractWithRegex(string sourceCode, string baseLink)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> ExtractWithParser(string sourceCode, string baseLink)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(sourceCode);

        // Extract href from <a> tags
        var hrefs = doc.DocumentNode.SelectNodes("//a[@href]")
                        ?.Select(a => a.Attributes["href"].Value.ToAbsoluteUrl(baseLink))
                    ?? Enumerable.Empty<string>();

        return hrefs.Distinct();
    }
}