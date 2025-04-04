using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    public interface IEventAttendanceService :IService<EventAttendance, EventAttendanceDTO>
    {
        Task<Result<List<Event>>> GetEventsForASingleUserAsync(int userId);
        Task<Result<List<User>>> GetUsersForASingleEventAsync(int eventId);
        Result<int> GetNumberOfUsersForASingleEvent(int eventId);

        Task<Result<EventAttendance>> UpdateEventAttendanceAsync(int userId, int eventId, EventAttendanceDTO dto);
        Task<Result<EventAttendance>> DeleteEventAttendanceAsync(int userId, int eventId);
    }
}
