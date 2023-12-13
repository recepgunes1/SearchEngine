namespace Shared.Entities;

public class ExtractedUrl
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Link { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}