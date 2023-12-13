using Microsoft.Extensions.Configuration;
using Nest;
using SearchHub.Infrastructure.Entities;
using Shared.DTOs;

namespace SearchHub.Infrastructure.Services;

public class SearchService : ISearchService
{
    private const string IndexName = "link_and_tags";
    private readonly ElasticClient _client;

    public SearchService(IConfiguration configuration)
    {
        var uri = new Uri(configuration.GetConnectionString("ElasticSearch") ?? throw new ArgumentNullException());
        var settings = new ConnectionSettings(uri)
            .DefaultIndex(IndexName)
            .PrettyJson();
        _client = new ElasticClient(settings);
    }

    public async Task<IEnumerable<SearchOutputDto>> Search(SearchUrlDto inputDto)
    {
        var skip = (inputDto.Page - 1) * inputDto.PageSize;
        var take = inputDto.PageSize;

        var searchResponse = await _client.SearchAsync<ElasticTag>(s => s
            .Index(IndexName)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(inputDto.Input)
                    .Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Tags)
                        .Field(p => p.InnerText)
                    )
                )
            )
            .Source(src => src
                .Includes(i => i
                    .Field(f => f.Link)
                    .Field(f => f.Title)
                    .Field(f => f.InnerText)
                )
            )
            .From(skip)
            .Size(take)
        );

        return !searchResponse.IsValid
            ? Enumerable.Empty<SearchOutputDto>()
            : searchResponse.Documents.Select(p => new SearchOutputDto
            {
                Title = p.Title,
                Link = p.Link,
                InnerText = p.InnerText?.Substring(0, 128) ?? string.Empty
            });
    }

    public async Task<IEnumerable<string>> SuggestTitle(string input)
    {
        var searchResponse = await _client.SearchAsync<ElasticTag>(s => s
            .Index(IndexName)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(input)
                    .Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Tags)
                        .Field(p => p.InnerText)
                    )
                )
            )
            .Source(src => src
                .Includes(i => i
                    .Field(f => f.Title)
                )
            )
        );

        return !searchResponse.IsValid
            ? Enumerable.Empty<string>()
            : searchResponse.Documents.Select(p => p.Title ?? string.Empty).Distinct();
    }
}