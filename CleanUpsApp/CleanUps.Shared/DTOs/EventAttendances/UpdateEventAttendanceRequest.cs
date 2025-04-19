using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    /// <summary>
    /// Data Transfer Object for updating an existing event attendance record.
    /// Typically used when checking in a user at an event.
    /// </summary>
    /// <param name="UserId">The ID of the user attending the event.</param>
    /// <param name="EventId">The ID of the event being attended.</param>
    /// <param name="CheckIn">The date and time when the user checked in at the event.</param>
    public record UpdateEventAttendanceRequest(int UserId, int EventId, DateTime CheckIn) : UpdateRequest;
}
