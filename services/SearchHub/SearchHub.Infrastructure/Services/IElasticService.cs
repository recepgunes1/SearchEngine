using SearchHub.Infrastructure.Entities;
using Shared.DTOs;

namespace SearchHub.Infrastructure.Services;

public interface IElasticService
{
    Task<IEnumerable<string>> SuggestTitle(string input);
    Task<IEnumerable<SearchOutputDto>> Search(SearchUrlDto inputDto);
    Task<string> IFeelLucky(string input);
    Task Insert(ElasticTag elasticTag);
}