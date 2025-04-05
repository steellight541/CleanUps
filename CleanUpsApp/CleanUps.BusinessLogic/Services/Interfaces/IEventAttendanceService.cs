using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IEventAttendanceService
    {
        Task<Result<List<EventAttendanceDTO>>> GetAllAsync();
        Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId);
        Task<Result<EventAttendanceDTO>> CreateAsync(EventAttendanceDTO entity);
        Task<Result<EventAttendanceDTO>> UpdateAsync(EventAttendanceDTO entity);
        Task<Result<EventAttendanceDTO>> DeleteAsync(EventAttendanceDTO entity);
    }
}
