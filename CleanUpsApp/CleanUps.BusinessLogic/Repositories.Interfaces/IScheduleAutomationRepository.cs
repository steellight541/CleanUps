using System.Threading.Tasks;

namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Defines methods for executing scheduled database maintenance tasks.
    /// </summary>
    public interface IScheduleAutomationRepository
    {
        /// <summary>
        /// Executes the nightly cleanup stored procedures to remove old/soft-deleted data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning true if successful, false otherwise.</returns>
        Task<bool> RunNightlyCleanupAsync();

        /// <summary>
        /// Executes the stored procedure to update event statuses based on current date/time.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning true if successful, false otherwise.</returns>
        Task<bool> RunStatusUpdateAsync();
    }
} 