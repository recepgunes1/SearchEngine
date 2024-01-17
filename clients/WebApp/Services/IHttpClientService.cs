namespace WebApp.Services;

public interface IHttpClientService
{
    Task<List<string>> GetSuggestions(string input);
    Task<string> GetLuckyUrl(string input);
}