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
    /// <summary>
    /// Provides extension methods for configuring the application's dependency injection container.
    /// Contains centralized registration of all services, repositories, validators, and converters used
    /// throughout the CleanUps application.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all application dependencies in the service collection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <param name="configuration">The application configuration containing connection strings and other settings.</param>
        /// <returns>The same service collection instance with all dependencies registered.</returns>
        /// <remarks>
        /// This method registers the following components:
        /// - Database context with SQL Server connection and retry policy
        /// - Repository implementations
        /// - Business logic services
        /// - Data validators
        /// - Data converters (DTO to model and vice versa)
        /// </remarks>
        public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<CleanUpsContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CleanUpsDb"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,                          // Retry up to 5 times
                        maxRetryDelay: TimeSpan.FromSeconds(30),  // Wait up to 30 seconds between retries
                        errorNumbersToAdd: null                  // Use default SQL error codes for retries
                    )
                )
            );

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