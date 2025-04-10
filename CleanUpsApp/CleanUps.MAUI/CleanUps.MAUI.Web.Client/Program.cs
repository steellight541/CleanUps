using CleanUps.MAUI.Shared;
using CleanUps.MAUI.Shared.Services;
using CleanUps.MAUI.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CleanUps.MAUI.Web.Client
{
    /// <summary>
    /// Entry point class for the Blazor WebAssembly client application.
    /// Configures and initializes the client-side of the Blazor hybrid application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point for the WebAssembly client application.
        /// Sets up services and initializes the client runtime.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add device-specific services used by the CleanUps.MAUI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            //Add Api Services
            builder.Services.AddApiServices();

            await builder.Build().RunAsync();
        }
    }
}
