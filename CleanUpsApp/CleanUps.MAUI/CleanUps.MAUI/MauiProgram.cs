using CleanUps.MAUI.Services;
using CleanUps.MAUI.Shared.Services;
using Microsoft.Extensions.Logging;

namespace CleanUps.MAUI
{
    /// <summary>
    /// Entry point class for the MAUI application.
    /// Configures and creates the MAUI application instance.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates and configures the MAUI application.
        /// Sets up services, fonts, and other application requirements.
        /// </summary>
        /// <returns>A configured MauiApp instance ready to run.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the CleanUps.MAUI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            //Add Api Services
            builder.Services.AddApiServices();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
