using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    internal interface IEventAttendanceRepository
    {
        Task<Result<List<EventAttendance>>> GetAllAsync();
        Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId);
        Task<Result<EventAttendance>> CreateAsync(EventAttendance eventAttendance);
        Task<Result<EventAttendance>> UpdateAsync(EventAttendance eventAttendance);
        Task<Result<EventAttendance>> DeleteAsync(EventAttendance eventAttendance);
    }
}
