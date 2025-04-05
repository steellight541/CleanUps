using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    public record EventAttendanceResponse(int UserId, int EventId, DateTime CheckIn) : Response;
}
