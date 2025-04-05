
using CleanUps.BusinessLogic.Converters;
using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators;
using CleanUps.BusinessLogic.Validators.Interfaces;
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
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IEventAttendanceRepository, EventAttendanceRepository>();


            // Register services
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IEventAttendanceService, EventAttendanceService>();

            // Register Validators
            services.AddScoped<IEventValidator, EventValidator>();
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IPhotoValidator, PhotoValidator>();
            services.AddScoped<IEventAttendanceValidator, EventAttendanceValidator>();


            // Register Converters
            services.AddSingleton<IEventConverter, EventConverter>();
            services.AddSingleton<IEventAttendanceConverter, EventAttendanceConverter>();
            services.AddSingleton<IPhotoConverter, PhotoConverter>();
            services.AddSingleton<IUserConverter, UserConverter>();

            return services;
        }
    }
}