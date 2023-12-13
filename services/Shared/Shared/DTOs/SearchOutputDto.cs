namespace Shared.DTOs;

public class SearchOutputDto
{
    public string? Title { get; set; }
    public string Link { get; set; } = null!;
    public string InnerText { get; set; } = null!;
}