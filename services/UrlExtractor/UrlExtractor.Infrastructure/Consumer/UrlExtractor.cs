using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Extensions;
using UrlExtractor.Infrastructure.Context;
using UrlExtractor.Infrastructure.Services;

namespace UrlExtractor.Infrastructure.Consumer;

public class UrlExtractor(AppDbContext dbContext, IExtractorService extractorService) : IConsumer<ExtractedUrl>
{
    public async Task Consume(ConsumeContext<ExtractedUrl> context)
    {
        try
        {
            var message = context.Message;
            var links = extractorService.ExtractWithParser(message.SourceCode.Decompress(), message.Link);
            foreach (var link in links)
            {
                if (await dbContext.ExtractedUrls.AnyAsync(p => p.Link == link)) continue;

                await dbContext.ExtractedUrls.AddAsync(new Shared.Entities.ExtractedUrl
                {
                    Link = link
                });

                await context.Publish(new DownloadedPage
                {
                    Link = link
                });
            }
        }
        finally
        {
            await dbContext.SaveChangesAsync();
        }
    }
}