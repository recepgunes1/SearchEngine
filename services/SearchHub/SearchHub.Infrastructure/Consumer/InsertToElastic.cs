using MassTransit;
using Microsoft.EntityFrameworkCore;
using SearchHub.Infrastructure.Context;
using SearchHub.Infrastructure.Entities;
using SearchHub.Infrastructure.Services;
using Shared.Entities;
using Shared.Events;

namespace SearchHub.Infrastructure.Consumer;

public class InsertToElastic(
    PageDownloaderDbContext pageDownloaderDbContext,
    TagExtractorDbContext tagExtractorDbContext,
    IElasticService elasticService) : IConsumer<InsertedElastic>
{
    public async Task Consume(ConsumeContext<InsertedElastic> context)
    {
        var message = context.Message;
        var page = await pageDownloaderDbContext.Pages.FirstOrDefaultAsync(p => p.Link == message.Link) ?? new Page();
        var tag = await tagExtractorDbContext.Tags.FirstOrDefaultAsync(p => p.Link == message.Link) ?? new LinkTags();
        var entity = new ElasticTag
        {
            Id = Guid.NewGuid().ToString(),
            InnerText = page.InnerText,
            Title = page.Title,
            Link = message.Link,
            Tags = tag.Tags.Split(";")
        };
        await elasticService.Insert(entity);
    }
}