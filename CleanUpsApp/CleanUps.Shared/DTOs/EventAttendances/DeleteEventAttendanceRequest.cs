using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    public record DeleteEventAttendanceRequest(int UserId, int EventId) : DeleteRequest;
}
