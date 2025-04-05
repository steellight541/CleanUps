using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IEventAttendanceService : IService<EventAttendanceResponse, CreateEventAttendanceRequest, UpdateEventAttendanceRequest, DeleteEventAttendanceRequest>
    {
        Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId);
        Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId);
    }
}
