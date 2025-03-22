using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Services;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<CleanUpsContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CleanUpsDb")));

            // Register repositories
            services.AddScoped<ICRUDRepository<Event>, EventRepository>();

            // Register services
            services.AddScoped<IEventProcessor, EventProcessor>();
            services.AddScoped<IEventValidator, EventValidator>();
            services.AddScoped<IEventMapper, EventMapper>();

            return services;
        }
    }
}