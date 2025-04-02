
using CleanUps.BusinessLogic.Converters;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Services;
using CleanUps.BusinessLogic.Validators;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.DataAccess.Repositories;
using CleanUps.Shared.DTOs;
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
            services.AddScoped<IRepository<Event>, EventRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IEventAttendanceRepository, EventAttendanceRepository>();


            // Register services
            services.AddScoped<IService<Event, EventDTO>, EventService>();
            services.AddScoped<IService<User, UserDTO>, UserService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IEventAttendanceService, EventAttendanceService>();

            // Register Validators
            services.AddScoped<IValidator<EventDTO>, EventValidator>();
            services.AddScoped<IValidator<UserDTO>, UserValidator>();
            services.AddScoped<IValidator<PhotoDTO>, PhotoValidator>();
            services.AddScoped<IEventAttendanceValidator, EventAttendanceValidator>();


            // Register Converters
            services.AddSingleton<IConverter<Event, EventDTO>, EventConverter>();
            services.AddSingleton<IConverter<EventAttendance, EventAttendanceDTO>, EventAttendanceConverter>();
            services.AddSingleton<IConverter<Photo, PhotoDTO>, PhotoConverter>();
            services.AddSingleton<IConverter<User, UserDTO>, UserConverter>();

            return services;
        }
    }
}