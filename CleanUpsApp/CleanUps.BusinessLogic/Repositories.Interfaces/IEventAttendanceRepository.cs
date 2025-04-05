using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    internal interface IEventAttendanceRepository : IRepository<EventAttendance>
    {
        Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId);
        Task<Result<EventAttendance>> DeleteAsync(DeleteEventAttendanceRequest eventAttendance);
    }
}
