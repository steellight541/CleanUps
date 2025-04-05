using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IEventAttendanceValidator
    {
        Result<EventAttendanceDTO> ValidateForCreate(EventAttendanceDTO createDto);
        Result<EventAttendanceDTO> ValidateForUpdate(EventAttendanceDTO updateDto);
        Result<bool> ValidateId(int id);

    }
}
