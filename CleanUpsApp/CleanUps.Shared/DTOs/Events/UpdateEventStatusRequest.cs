using CleanUps.Shared.DTOs.Enums;

namespace CleanUps.Shared.DTOs.Events
{
    /// <summary>
    /// Data Transfer Object for requesting an update to an event's status.
    /// </summary>
    /// <param name="EventId">The unique identifier for the event whose status is to be updated.</param>
    /// <param name="NewStatus">The new status to set for the event.</param>
    public record UpdateEventStatusRequest(int EventId, StatusDTO NewStatus);
} 