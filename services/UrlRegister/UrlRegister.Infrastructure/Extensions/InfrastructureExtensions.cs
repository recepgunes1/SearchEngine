using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DTOs;
using UrlRegister.Infrastructure.Context;
using UrlRegister.Infrastructure.Validators;

namespace UrlRegister.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection LoadInfrastructureLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(p => p.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddTransient<IValidator<RegisterUrlDto>, RegisterUrlDtoValidator>();
        services.AddMassTransit(x => x.UsingRabbitMq((_, cfg) =>
        {
            cfg.Host(configuration["RabbitMQ:Host"], Convert.ToUInt16(configuration["RabbitMQ:Port"]),
                configuration["RabbitMQ:VirtualHost"],
                u =>
                {
                    u.Username(configuration["RabbitMQ:Username"]);
                    u.Username(configuration["RabbitMQ:Password"]);
                });
        }));
        return services;
    }
}