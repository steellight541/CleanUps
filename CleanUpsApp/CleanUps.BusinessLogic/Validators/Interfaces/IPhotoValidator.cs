using CleanUps.Shared.DTOs.Photos;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating photo-related requests.
    /// Ensures that photo data meets the application's business rules before processing.
    /// </summary>
    internal interface IPhotoValidator : IValidator<CreatePhotoRequest, UpdatePhotoRequest>;
}
