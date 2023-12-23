using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PageDownloader.Infrastructure.Consumers;
using PageDownloader.Infrastructure.Context;

namespace PageDownloader.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<PageDownloaderConsumer>();
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