using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchHub.Infrastructure.Services;

namespace SearchHub.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IElasticService, ElasticService>();
        return services;
    }
}