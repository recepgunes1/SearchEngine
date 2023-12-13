using PageDownloader.Infrastructure.Extensions;

var host = Host.CreateApplicationBuilder(args);

host.Services.LoadInfrastructureLayer(host.Configuration);

host.Build().Run();