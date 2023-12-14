namespace SearchHub.Infrastructure.Entities;

public class ElasticTag
{
    public string Id { get; set; } = null!;
    public string? Title { get; set; }
    public string Link { get; set; } = null!;
    public string? InnerText { get; set; }
    public string[] Tags { get; set; } = null!;
}
