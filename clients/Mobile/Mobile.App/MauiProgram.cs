using Microsoft.Extensions.Logging;
using Mobile.Infrastructure.Extensions;

namespace Mobile.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.LoadInfrastructureLayer();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<RegisterPage>();

        return builder.Build();
    }
}