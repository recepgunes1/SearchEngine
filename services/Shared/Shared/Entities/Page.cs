namespace Shared.Entities;

public class Page
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Link { get; set; } = null!;
    public string? Title { get; set; }
    public string? InnerText { get; set; }
    public byte[] CompressedSourceCode { get; set; } = null!;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
}