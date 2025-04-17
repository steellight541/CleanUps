using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing user operations.
    /// Provides functionality for retrieving, creating, updating, and deleting users in the system.
    /// </summary>
    public interface IUserService : IService<UserResponse, CreateUserRequest, UpdateUserRequest, DeleteUserRequest>
    {
        /// <summary>
        /// Changes the password for a specified user.
        /// </summary>
        /// <param name="changeRequest">The request containing the user ID and new password.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest changeRequest);
    }
}
