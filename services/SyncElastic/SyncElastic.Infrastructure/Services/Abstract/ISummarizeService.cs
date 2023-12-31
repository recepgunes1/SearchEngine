namespace SyncElastic.Infrastructure.Services.Abstract;

public interface ISummarizeService
{
    Task<string> SummarizeAsync(string input);
}