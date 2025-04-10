using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    /// <summary>
    /// Data Transfer Object for creating a new event attendance record.
    /// Used when a user registers to attend an event.
    /// </summary>
    /// <param name="UserId">The ID of the user who will attend the event.</param>
    /// <param name="EventId">The ID of the event the user will attend.</param>
    public record CreateEventAttendanceRequest(int UserId, int EventId) : CreateRequest;
}
