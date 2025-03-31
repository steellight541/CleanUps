
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Mappers;
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
            services.AddScoped<IRepository<Photo>, PhotoRepository>();
            services.AddScoped<IRepository<EventAttendance>, EventAttendanceRepository>();


            // Register services
            // Services
            services.AddScoped<IService<Event, EventDTO>, EventService>();
            services.AddScoped<IService<User, UserDTO>, UserService>();
            services.AddScoped<IService<Photo, PhotoDTO>, PhotoService>();
            services.AddScoped<IService<EventAttendance, EventAttendanceDTO>, EventAttendanceService>();

            // Validators
            services.AddScoped<IValidator<EventDTO>, EventValidator>();
            services.AddScoped<IValidator<UserDTO>, UserValidator>();
            services.AddScoped<IValidator<PhotoDTO>, PhotoValidator>();
            services.AddScoped<IValidator<EventAttendanceDTO>, EventAttendanceValidator>();


            // Mappers
            services.AddSingleton<IMapper<Event, EventDTO>, EventMapper>();
            services.AddSingleton<IMapper<EventAttendance, EventAttendanceDTO>, EventAttendanceMapper>();
            services.AddSingleton<IMapper<Photo, PhotoDTO>, PhotoMapper>();
            services.AddSingleton<IMapper<User, UserDTO>, UserMapper>();

            return services;
        }
    }
}