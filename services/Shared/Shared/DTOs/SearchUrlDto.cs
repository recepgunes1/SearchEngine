namespace Shared.DTOs;

public class SearchUrlDto
{
    public string Input { get; set; } = null!;

    public int Page { get; set; }

    public int PageSize { get; set; }
}