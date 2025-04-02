using CleanUps.Shared.ClientServices;
using Microsoft.Extensions.Logging;

namespace CleanUps.MAUI;

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
            });

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://cleanup-rest-azc5hnbgdca3hwa6.westeurope-01.azurewebsites.net") });// Your API base URL

        builder.Services.AddScoped<EventApiService>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
