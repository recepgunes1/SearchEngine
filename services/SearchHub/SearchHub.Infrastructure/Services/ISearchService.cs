using Shared.DTOs;

namespace SearchHub.Infrastructure.Services;

public interface ISearchService
{
    Task<IEnumerable<string>> SuggestTitle(string input);
    Task<IEnumerable<SearchOutputDto>> Search(SearchUrlDto inputDto);
}