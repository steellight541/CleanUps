using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing event attendance operations.
    /// Provides functionality for tracking user participation in events and retrieving attendance information.
    /// </summary>
    public interface IEventAttendanceService : IService<EventAttendanceResponse, CreateEventAttendanceRequest, UpdateEventAttendanceRequest, DeleteEventAttendanceRequest>
    {
        /// <summary>
        /// Retrieves all events that a specific user has attended or registered for.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve events for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="EventResponse"/> objects if successful, or an error message if the operation fails.</returns>
        Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId);

        /// <summary>
        /// Retrieves all users who have attended or registered for a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve users for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UserResponse"/> objects if successful, or an error message if the operation fails.</returns>
        Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId);
    }
}
