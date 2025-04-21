using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.DataAccess.DatabaseHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CleanUps.DataAccess.Repositories
{
    /// <summary>
    /// Repository for executing scheduled database automation tasks via stored procedures.
    /// </summary>
    public class ScheduleAutomationRepository : IScheduleAutomationRepository
    {
        private readonly CleanUpsContext _context;
        private readonly ILogger<ScheduleAutomationRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleAutomationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger for recording operations and errors.</param>
        public ScheduleAutomationRepository(CleanUpsContext context, ILogger<ScheduleAutomationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Executes the nightly cleanup stored procedures (DeleteOldUsers, DeleteOldEvents, DeleteOldEventAttendances).
        /// </summary>
        /// <returns>True if all procedures executed successfully, false otherwise.</returns>
        public async Task<bool> RunNightlyCleanupAsync()
        {
            // Step 1: Log the start of the operation.
            _logger.LogInformation("Starting nightly cleanup procedures...");
            try
            {
                // Step 2: Execute the stored procedure to delete old users.
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteOldUsers");
                _logger.LogInformation("Executed DeleteOldUsers procedure.");

                // Step 3: Execute the stored procedure to delete old events.
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteOldEvents");
                _logger.LogInformation("Executed DeleteOldEvents procedure.");

                // Step 4: Execute the stored procedure to delete old event attendances.
                await _context.Database.ExecuteSqlRawAsync("EXEC DeleteOldEventAttendances");
                _logger.LogInformation("Executed DeleteOldEventAttendances procedure.");

                // Step 5: Log successful completion.
                _logger.LogInformation("Nightly cleanup procedures completed successfully.");
                // Step 6: Return true indicating success.
                return true;
            }
            catch (Exception ex)
            {
                // Step 7: Log any exception that occurred during the process.
                _logger.LogError(ex, "An error occurred during the nightly cleanup procedures.");
                // Step 8: Return false indicating failure.
                return false;
            }
        }

        /// <summary>
        /// Executes the stored procedure to update event statuses.
        /// </summary>
        /// <returns>True if the procedure executed successfully, false otherwise.</returns>
        public async Task<bool> RunStatusUpdateAsync()
        {
            // Step 1: Log the start of the operation.
            _logger.LogInformation("Starting event status update procedure...");
            try
            {
                // Step 2: Execute the stored procedure to update event statuses.
                await _context.Database.ExecuteSqlRawAsync("EXEC UpdateEventStatuses");
                
                // Step 3: Log successful completion.
                _logger.LogInformation("Event status update procedure completed successfully.");
                // Step 4: Return true indicating success.
                return true;
            }
            catch (Exception ex)
            {
                // Step 5: Log any exception that occurred during the process.
                _logger.LogError(ex, "An error occurred during the event status update procedure.");
                // Step 6: Return false indicating failure.
                return false;
            }
        }
    }
} 