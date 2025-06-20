using CleanUps.MAUI.Shared.AuthServices;
using CleanUps.Shared.ClientServices;
using CleanUps.Shared.ClientServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CleanUps.MAUI.Shared.Authorization.AuthInterfaces;
using CleanUps.MAUI.Shared.Authorization.AuthServices;

namespace CleanUps.MAUI.Shared.Services
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
                client.BaseAddress = new Uri("https://cleanups.azurewebsites.net");
                //https://cleanups.azurewebsites.net
                //https://localhost:7128/
            });

            // Add simplified session services
            services.AddScoped<ISessionService, SessionStorageService>();
            services.AddScoped<IAccessService, UserSessionService>();

            services.AddScoped<IAuthApiService, AuthApiService>();
            services.AddScoped<IEventApiService, EventApiService>();
            services.AddScoped<IEventAttendanceApiService, EventAttendanceApiService>();
            services.AddScoped<IPhotoApiService, PhotoApiService>();
            services.AddScoped<IUserApiService, UserApiService>();
        }
    }
}
