using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    public interface IEventApiService : IApiService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>
    {
        /// <summary>
        /// Updates the status of an existing event via the API and returns the updated event.
        /// </summary>
        /// <param name="request">The request containing the event ID and the new status.</param>
        /// <returns>A Result containing the updated <see cref="EventResponse"/> if successful, or an error message otherwise.</returns>
        Task<Result<EventResponse>> UpdateStatusAsync(UpdateEventStatusRequest request);
    }
}
