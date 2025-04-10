using CleanUps.MAUI.Shared.Services;
using CleanUps.MAUI.Web.Components;
using CleanUps.MAUI.Web.Services;
using CleanUps.MAUI.Shared;

namespace CleanUps.MAUI
{
    /// <summary>
    /// Entry point class for the ASP.NET Core web application.
    /// Configures and runs the web server for the Blazor hybrid application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point for the web application.
        /// Sets up the web host, configures services, middleware, and starts the application.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Add device-specific services used by the CleanUps.MAUI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            //Add Api Services
            builder.Services.AddApiServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(
                    typeof(CleanUps.MAUI.Shared._Imports).Assembly,
                    typeof(CleanUps.MAUI.Web.Client._Imports).Assembly);

            app.Run();
        }
    }
}
