using Microsoft.Extensions.Configuration;
using Nest;
using Shared.Entities;
using SyncElastic.Infrastructure.Services.Abstract;

namespace SyncElastic.Infrastructure.Services.Concrete;

public class ElasticService : IElasticService
{
    private const string IndexName = "link_and_tags";
    private readonly ElasticClient _client;

    public ElasticService(IConfiguration configuration)
    {
        var uri = new Uri(configuration.GetConnectionString("ElasticSearch") ?? throw new ArgumentNullException());
        var settings = new ConnectionSettings(uri)
            .DefaultIndex(IndexName)
            .PrettyJson();
        _client = new ElasticClient(settings);
    }

    public async Task Insert(ElasticTag elasticTag)
    {
        var response = await _client.IndexDocumentAsync(elasticTag);
        if (!response.IsValid)
            // Handle the error, log it, or throw an exception
            throw new InvalidOperationException($"Failed to insert document: {response.OriginalException.Message}");
    }
}