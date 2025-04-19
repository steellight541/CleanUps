using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing event operations.
    /// Provides functionality for retrieving, creating, updating, and deleting events in the system.
    /// </summary>
    public interface IEventService : IService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>
    {
        /// <summary>
        /// Updates the status of a specific event and returns the updated event.
        /// </summary>
        /// <param name="request">The request containing the event ID and the new status.</param>
        /// <returns>A Result containing the updated <see cref="EventResponse"/> if successful, or an error message otherwise.</returns>
        Task<Result<EventResponse>> UpdateStatusAsync(UpdateEventStatusRequest request);
    }
}
