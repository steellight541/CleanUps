using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for EventAttendance entity data access operations.
    /// Provides CRUD operations and specialized queries for user attendance at events.
    /// </summary>
    internal interface IEventAttendanceRepository : IRepository<EventAttendance>
    {
        /// <summary>
        /// Retrieves all events that a specific user has attended or registered for.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve events for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of events if successful, or an error message if the operation fails.</returns>
        Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId);
        
        /// <summary>
        /// Retrieves all users who have attended or registered for a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve users for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of users if successful, or an error message if the operation fails.</returns>
        Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId);
        
        /// <summary>
        /// Deletes an event attendance record based on the provided information.
        /// </summary>
        /// <param name="eventAttendance">The DTO containing information about which attendance record to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted attendance record if successful, or an error message if the operation fails.</returns>
        Task<Result<EventAttendance>> DeleteAsync(DeleteEventAttendanceRequest eventAttendance);
    }
}
