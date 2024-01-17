using Web.Infrastructure.Models;

namespace Web.Infrastructure.Services.Abstracts;

public interface IHttpClientService
{
    Task<List<ResultModel>> GetResults(string input, int page, int pageSize);
    Task<List<string>> GetSuggestions(string input);
    Task<string> GetLuckyUrl(string input);
    Task<string> Register(string input);
}