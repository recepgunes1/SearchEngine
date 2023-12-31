using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Events;
using SyncElastic.Infrastructure.Contexts;
using SyncElastic.Infrastructure.Services.Abstract;

namespace SyncElastic.Infrastructure.Consumers;

public class InsertToElastic(
    PageDownloaderDbContext pageDownloaderDbContext,
    TagExtractorDbContext tagExtractorDbContext,
    IElasticService elasticService,
    ISummarizeService summarizeService) : IConsumer<InsertedElastic>
{
    public async Task Consume(ConsumeContext<InsertedElastic> context)
    {
        var message = context.Message;
        var page = await pageDownloaderDbContext.Pages.FirstOrDefaultAsync(p => p.Link == message.Link) ?? new Page();
        var tag = await tagExtractorDbContext.Tags.FirstOrDefaultAsync(p => p.Link == message.Link) ?? new LinkTags();
        var summary = string.IsNullOrEmpty(page.InnerText)
            ? string.Empty
            : await summarizeService.SummarizeAsync(page.InnerText);
        var entity = new ElasticTag
        {
            Id = Guid.NewGuid().ToString(),
            InnerText = page.InnerText,
            Title = page.Title,
            Link = message.Link,
            Tags = tag.Tags.Split(";"),
            Summary = summary
        };
        await elasticService.Insert(entity);
    }
}