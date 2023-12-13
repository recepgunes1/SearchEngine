using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MassTransit;
using Shared.Entities;
using Shared.Events;
using Shared.Extensions;
using TagExtractor.Infrastructure.Context;

namespace TagExtractor.Infrastructure.Consumer;

public class TagExtractor(AppDbContext dbContext) : IConsumer<ExtractedTag>
{
    public async Task Consume(ConsumeContext<ExtractedTag> context)
    {
        var message = context.Message;
        var doc = new HtmlDocument();
        doc.LoadHtml(message.SourceCode.Decompress());
        var tags = Regex.Matches(doc.DocumentNode.InnerText, @"\w+")
            .Select(match => match.Value.ToLower())
            .Distinct()
            .ToArray();
        await dbContext.AddAsync(new LinkTags
        {
            Link = message.Link,
            Tags = string.Join(';', tags)
        });
        await dbContext.SaveChangesAsync();
    }
}