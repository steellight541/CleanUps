using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces
{
    internal interface IEventAttendanceRepository : IRepository<EventAttendance>
    {
        Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId);
        Result<int> GetAttendanceCountByEventId(int eventId);
        Task<Result<EventAttendance>> DeleteAttendanceAsync(int userId, int eventId);
    }
}
