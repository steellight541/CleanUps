using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating event-related requests.
    /// Ensures that event data meets the application's business rules before processing.
    /// </summary>
    internal interface IEventValidator : IValidator<CreateEventRequest, UpdateEventRequest>
    {
        /// <summary>
        /// Validates an UpdateEventStatusRequest before updating an event's status.
        /// Ensures the ID and status are valid.
        /// </summary>
        /// <param name="request">The UpdateEventStatusRequest to validate.</param>
        /// <returns>A Result indicating success or failure with an error message.</returns>
        Result<bool> ValidateForStatusUpdate(UpdateEventStatusRequest request);
    }
}
