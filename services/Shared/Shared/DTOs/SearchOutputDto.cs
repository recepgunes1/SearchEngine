namespace Shared.DTOs;

public class SearchOutputDto
{
    public string? Title { get; set; }
    public string Link { get; set; } = null!;
    public string Explanation { get; set; } = null!;
}