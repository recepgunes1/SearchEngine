namespace Shared.Entities;

public class LinkTags
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Link { get; set; } = null!;
    public string Tags { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}