using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.Shared.DTOs.EventAttendances
{
    public record CreateEventAttendanceRequest(int UserId, int EventId) : CreateRequest;
}
