using CleanUps.Shared.ClientServices;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.MAUI.Shared
{
    public static class ServiceExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {

            services.AddHttpClient("CleanupsApi", client =>
            {
                client.BaseAddress = new Uri("https://cleanups-api-enbrcrevatgmhke7.canadacentral-01.azurewebsites.net");
            });

            services.AddScoped<EventApiService>();
            services.AddScoped<EventAttendanceApiService>();
            services.AddScoped<PhotoApiService>();
            services.AddScoped<UserApiService>();

        }
    }
}
