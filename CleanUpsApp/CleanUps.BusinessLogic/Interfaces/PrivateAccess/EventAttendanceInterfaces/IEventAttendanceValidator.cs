using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces
{
    internal interface IEventAttendanceValidator : IValidator<EventAttendanceDTO>
    {
        Result<EventAttendanceDTO> ValidateEventAttendanceForUpdate(int userId, int eventId, EventAttendanceDTO dto);
    }
}
