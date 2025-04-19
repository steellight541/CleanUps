using CleanUps.Shared.DTOs.EventAttendances;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating event attendance-related requests.
    /// Ensures that event attendance data meets the application's business rules before processing.
    /// </summary>
    internal interface IEventAttendanceValidator : IValidator<CreateEventAttendanceRequest, UpdateEventAttendanceRequest>
    {
    }
}
