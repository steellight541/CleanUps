using CleanUps.MAUI.Shared.Services;
using CleanUps.MAUI.Web.Components;
using CleanUps.MAUI.Web.Services;
using CleanUps.MAUI.Shared;


namespace CleanUps.MAUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Add device-specific services used by the CleanUps.MAUI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();
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
