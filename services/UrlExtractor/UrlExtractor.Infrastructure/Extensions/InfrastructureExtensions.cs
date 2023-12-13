using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlExtractor.Infrastructure.Context;
using UrlExtractor.Infrastructure.Services;

namespace UrlExtractor.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddScoped<IExtractorService, ExtractorService>();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<Consumer.UrlExtractor>();
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