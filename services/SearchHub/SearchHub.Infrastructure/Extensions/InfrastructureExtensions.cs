using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchHub.Infrastructure.Context;
using SearchHub.Infrastructure.Services;

namespace SearchHub.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PageDownloaderDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("PageDownloaderDatabase")));
        services.AddDbContext<TagExtractorDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("TagExtractorDatabase")));
        services.AddScoped<IElasticService, ElasticService>();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<Consumer.InsertToElastic>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], Convert.ToUInt16(configuration["RabbitMQ:Port"]),
                    configuration["RabbitMQ:VirtualHost"],
                    u =>
                    {
                        u.Username(configuration["RabbitMQ:Username"]);
                        u.Password(configuration["RabbitMQ:Password"]);
                    });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
