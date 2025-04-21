using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Event entity data access operations.
    /// Provides CRUD operations for manipulating event data in the database.
    /// </summary>
    internal interface IEventRepository : IRepository<Event>
    {

        /// <summary>
        /// Updates the status of a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to update.</param>
        /// <param name="newStatusId">The ID of the new status to set.</param>
        /// <returns>A Result indicating success (true) or failure (false).</returns>
        Task<Result<Event>> UpdateStatusAsync(int eventId, int newStatusId);
    }
}
