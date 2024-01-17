using Mobile.Infrastructure.Services.Abstracts;
using Mobile.Infrastructure.Services.Concretes;

namespace Mobile.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IHttpClientService, HttpClientService>();
        return services;
    }
}