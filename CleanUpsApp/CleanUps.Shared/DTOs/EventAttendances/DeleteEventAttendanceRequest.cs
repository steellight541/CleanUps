using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    /// <summary>
    /// Data Transfer Object for deleting an event attendance record.
    /// Used when a user cancels their attendance at an event.
    /// </summary>
    /// <param name="UserId">The ID of the user whose attendance record should be deleted.</param>
    /// <param name="EventId">The ID of the event from which the user's attendance should be removed.</param>
    public record DeleteEventAttendanceRequest(int UserId, int EventId) : DeleteRequest;
}
