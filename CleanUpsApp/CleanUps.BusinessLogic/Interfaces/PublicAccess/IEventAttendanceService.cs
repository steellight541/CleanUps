using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IEventAttendanceService :IService<EventAttendance, EventAttendanceDTO>
    {
        Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId);
        Result<int> GetAttendanceCountByEventId(int eventId);

        Task<Result<EventAttendance>> UpdateAttendanceAsync(int userId, int eventId, EventAttendanceDTO dto);
        Task<Result<EventAttendance>> DeleteAttendanceAsync(int userId, int eventId);
    }
}
