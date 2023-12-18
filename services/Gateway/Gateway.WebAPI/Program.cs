using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(string.IsNullOrEmpty(builder.Environment.EnvironmentName)
    ? "ocelot.json"
    : $"ocelot.{builder.Environment.EnvironmentName}.json");

builder.Services.AddOcelot().AddKubernetes();

var app = builder.Build();

await app.UseOcelot();

app.Run();