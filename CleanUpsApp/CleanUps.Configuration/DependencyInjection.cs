using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Services.Mappers;
using CleanUps.BusinessLogic.Services.Processors;
using CleanUps.BusinessLogic.Services.Validators;
using CleanUps.DataAccess.DatabaseHub;
using CleanUps.DataAccess.Repositories;
using CleanUps.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanUps.Configuration
{
    /// <summary>
    /// Provides extension methods for registering application dependencies in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the CleanUps application dependencies to the provided <see cref="IServiceCollection"/>.
        /// This method registers interface and implementation of database contexts, repositories, processors, validators, and mappers.
        /// </summary>
        /// <param name="services">
        /// The service collection to which dependencies will be added.
        /// </param>
        /// <param name="configuration">
        /// The application configuration containing settings like connection strings.
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<CleanUpsContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CleanUpsDb")));

            // Register repositories
            services.AddScoped<ICRUDRepository<Event>, EventRepository>();
            services.AddScoped<ICRUDRepository<User>, UserRepository>();


            // Register services
            // Processors
            services.AddScoped<IDTOProcessor<EventDTO>, EventProcessor>();

            // Validators
            services.AddScoped<IDTOValidator<EventDTO>, EventValidator>();

            // Mappers
            services.AddSingleton<IMapper<Event, EventDTO>, EventMapper>();
            services.AddSingleton<IMapper<EventAttendance, EventAttendanceDTO>, EventAttendanceMapper>();
            services.AddSingleton<IMapper<Photo, PhotoDTO>, PhotoMapper>();
            services.AddSingleton<IMapper<User, UserDTO>, UserMapper>();

            return services;
        }
    }
}