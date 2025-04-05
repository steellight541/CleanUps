using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    public record UpdateEventAttendanceRequest(int UserId, int EventId, DateTime CheckIn) : UpdateRequest;
}
