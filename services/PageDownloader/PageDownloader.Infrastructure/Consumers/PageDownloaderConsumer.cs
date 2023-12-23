using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PageDownloader.Infrastructure.Context;
using Shared.Entities;
using Shared.Events;
using Shared.Extensions;
using ExtractedUrl = Shared.Events.ExtractedUrl;

namespace PageDownloader.Infrastructure.Consumers;

public class PageDownloaderConsumer(AppDbContext dbContext) : IConsumer<DownloadedPage>
{
    public async Task Consume(ConsumeContext<DownloadedPage> context)
    {
        var message = context.Message;
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
        client.DefaultRequestHeaders.Add("Referer", message.Link);
        client.Timeout = TimeSpan.FromSeconds(30);
        var sourceCode = await client.GetStringAsync(message.Link);
        var compressedSourceCode = sourceCode.Compress();
        var doc = new HtmlDocument();
        doc.LoadHtml(sourceCode);
        var title = doc.DocumentNode.SelectSingleNode("//title").InnerText ?? string.Empty;
        var innerText = doc.DocumentNode.SelectSingleNode("//body").InnerText ?? string.Empty;
        innerText = Regex.Replace(innerText, @"\s+", " ");

        // Check if the page already exists in the database
        if (await dbContext.Pages.AnyAsync(x => x.Link == message.Link)) return;

        // Save the extracted information to the database
        await dbContext.Pages.AddAsync(new Page
        {
            Link = message.Link,
            Title = title,
            InnerText = innerText,
            CompressedSourceCode = compressedSourceCode
        });

        await dbContext.SaveChangesAsync();

        // Publish extracted data
        await context.Publish(new ExtractedTag
        {
            SourceCode = compressedSourceCode,
            Link = message.Link
        });
        await context.Publish(new ExtractedUrl
        {
            SourceCode = compressedSourceCode,
            Link = message.Link
        });
    }
}