using CleanUps.Shared.DTOs.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating user-related requests.
    /// Ensures that user data meets the application's business rules before processing.
    /// </summary>
    internal interface IUserValidator : IValidator<CreateUserRequest, UpdateUserRequest>;
}
