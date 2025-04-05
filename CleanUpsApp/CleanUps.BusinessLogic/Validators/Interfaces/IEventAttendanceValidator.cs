using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IEventAttendanceValidator
    {
        Result<EventAttendanceDTO> ValidateForCreate(EventAttendanceDTO dto);
        Result<EventAttendanceDTO> ValidateEventAttendanceForUpdate(int userId, int eventId, EventAttendanceDTO dto);
        Result<bool> ValidateId(int id);

    }
}
