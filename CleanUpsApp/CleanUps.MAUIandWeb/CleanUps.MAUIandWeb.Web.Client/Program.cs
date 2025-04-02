using CleanUps.MAUIandWeb.Shared.Services;
using CleanUps.MAUIandWeb.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CleanUps.MAUIandWeb.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add device-specific services used by the CleanUps.MAUIandWeb.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            await builder.Build().RunAsync();
        }
    }
}
