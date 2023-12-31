using Shared.Entities;

namespace SyncElastic.Infrastructure.Services.Abstract;

public interface IElasticService
{
    Task Insert(ElasticTag elasticTag);
}