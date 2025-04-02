using CleanUps.MAUI.Shared.Services;
using CleanUps.MAUI.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CleanUps.MAUI.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add device-specific services used by the CleanUps.MAUI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            await builder.Build().RunAsync();
        }
    }
}
