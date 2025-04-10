using CleanUps.Shared.DTOs.Events;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating event-related requests.
    /// Ensures that event data meets the application's business rules before processing.
    /// </summary>
    internal interface IEventValidator : IValidator<CreateEventRequest, UpdateEventRequest>;
}
