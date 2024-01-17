using WebApp.Components;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();