using CleanUps.MAUI.Shared.Services;
using CleanUps.MAUI.Shared.Services.Interfaces;
using CleanUps.Shared.ClientServices;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.MAUI.Shared
{
    /// <summary>
    /// Provides extension methods for configuring services in the MAUI application.
    /// Contains methods to register API client services for dependency injection.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds API client services to the service collection.
        /// Configures the HttpClient with the base address for the CleanUps API and 
        /// registers all API service classes for dependency injection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddHttpClient("CleanupsApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7128/");
            });

            // Add simplified session services
            services.AddScoped<ISessionService, LocalStorageSessionService>();

            services.AddScoped<EventApiService>();
            services.AddScoped<EventAttendanceApiService>();
            services.AddScoped<PhotoApiService>();
            services.AddScoped<UserApiService>();
            services.AddScoped<IAccessService, AccessService>();
        }
    }
}
