using Microsoft.Extensions.DependencyInjection;
using Web.Infrastructure.Services.Abstracts;
using Web.Infrastructure.Services.Concretes;

namespace Web.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IHttpClientService, HttpClientService>();
        return services;
    }
}