using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.Events
{
    /// <summary>
    /// Data Transfer Object for deleting an existing event.
    /// Contains the identifier of the event to be deleted.
    /// </summary>
    /// <param name="EventId">The unique identifier of the event to be deleted.</param>
    public record DeleteEventRequest(int EventId) : DeleteRequest;
}
