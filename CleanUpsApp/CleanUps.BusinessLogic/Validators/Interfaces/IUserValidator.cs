using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating user-related requests.
    /// Ensures that user data meets the application's business rules before processing.
    /// </summary>
    internal interface IUserValidator : IValidator<CreateUserRequest, UpdateUserRequest>
    {
        /// <summary>
        /// Validates a ChangePasswordRequest.
        /// </summary>
        /// <param name="changePasswordRequestDto">The ChangePasswordRequest DTO to validate.</param>
        /// <returns>A Result indicating success or failure with an error message.</returns>
        Result<bool> ValidateForPasswordChange(ChangePasswordRequest changePasswordRequestDto);
    }
}
