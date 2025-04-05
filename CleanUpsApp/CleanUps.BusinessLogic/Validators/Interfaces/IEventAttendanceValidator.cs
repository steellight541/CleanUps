using CleanUps.Shared.DTOs.EventAttendances;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IEventAttendanceValidator : IValidator<CreateEventAttendanceRequest, UpdateEventAttendanceRequest>
    {
    }
}
