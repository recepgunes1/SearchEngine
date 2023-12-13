using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(builder.Environment.IsProduction() ? "ocelot.json" : "ocelot.Development.json");

builder.Services.AddOcelot().AddKubernetes();

var app = builder.Build();

await app.UseOcelot();

app.Run();