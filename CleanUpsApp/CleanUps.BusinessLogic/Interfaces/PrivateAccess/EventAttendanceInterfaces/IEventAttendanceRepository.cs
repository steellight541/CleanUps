using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces
{
    internal interface IEventAttendanceRepository : IRepository<EventAttendance>
    {
        Task<Result<List<Event>>> GetEventsForASingleUserAsync(int userId);
        Task<Result<List<User>>> GetUsersForASingleEventAsync(int eventId);
        Task<Result<EventAttendance>> DeleteEventAttendanceAsync(int eventId, int userId);
    }
}
