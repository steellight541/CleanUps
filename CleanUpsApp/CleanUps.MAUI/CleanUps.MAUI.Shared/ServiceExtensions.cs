using CleanUps.MAUI.Shared.Auth;
using CleanUps.Shared.ClientServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.MAUI.Shared
{
    public static class ServiceExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            // **** START AUTH CONFIG ****
            // Add Authorization Core services
            services.AddAuthorizationCore();
            // Register your custom AuthenticationStateProvider
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            // **** END AUTH CONFIG ****

            //Add httpClient
            //services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://cleanup-rest-azc5hnbgdca3hwa6.westeurope-01.azurewebsites.net") });

            services.AddScoped(sp => {
                // Get the AuthStateProvider to potentially help configure HttpClient if needed later
                var authStateProvider = sp.GetRequiredService<AuthenticationStateProvider>();
                // Or inject IHttpContextAccessor for web scenarios

                // The HttpClient needs the BaseAddress
                var client = new HttpClient { BaseAddress = new Uri("https://cleanup-rest-azc5hnbgdca3hwa6.westeurope-01.azurewebsites.net") };

                // *** Important: Get the AuthStateProvider instance here AFTER it's potentially set the default headers ***
                // This relies on the DI scope resolving AuthStateProvider first when HttpClient is requested, which *might* work.
                // A DelegatingHandler is cleaner.
                sp.GetRequiredService<AuthenticationStateProvider>(); // Trigger AuthStateProvider resolution

                return client;
            });

            //Add ClientServices
            services.AddScoped<EventApiService>();
            services.AddScoped<EventAttendanceApiService>();
            services.AddScoped<PhotoApiService>();
            services.AddScoped<UsersApiService>();
            services.AddScoped<AuthApiService>();
        }
    }
}
