using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for user authentication operations.
    /// Provides methods for user login and session management.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user based on provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>A Result containing the logged-in user information if successful, or an error message if authentication fails.</returns>
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
    }
}