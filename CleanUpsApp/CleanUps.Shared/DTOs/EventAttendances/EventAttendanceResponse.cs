using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    /// <summary>
    /// Data Transfer Object representing an Event Attendance record in API responses.
    /// Contains information about a user's attendance at a specific event.
    /// </summary>
    /// <param name="UserId">The ID of the user attending the event.</param>
    /// <param name="EventId">The ID of the event being attended.</param>
    /// <param name="CheckIn">The date and time when the user checked in at the event, or null if they haven't checked in yet.</param>
    /// <param name="CreatedDate">The date and time when the attendance record was created.</param>
    public record EventAttendanceResponse(int UserId, int EventId, DateTime CheckIn, DateTime CreatedDate) : Response;
}
