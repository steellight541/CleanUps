using CleanUps.Shared.ClientServices;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.MAUI.Shared
{
    public static class ServiceExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://cleanup-rest-azc5hnbgdca3hwa6.westeurope-01.azurewebsites.net") });
            services.AddScoped<EventApiService>();
            services.AddScoped<EventAttendanceApiService>();
            services.AddScoped<PhotoApiService>();
            services.AddScoped<UsersApiService>();

        }
    }
}
