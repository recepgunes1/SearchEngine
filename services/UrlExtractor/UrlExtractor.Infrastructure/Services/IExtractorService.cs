namespace UrlExtractor.Infrastructure.Services;

public interface IExtractorService
{
    IEnumerable<string> ExtractWithRegex(string sourceCode, string baseLink);
    IEnumerable<string> ExtractWithParser(string sourceCode, string baseLink);
}