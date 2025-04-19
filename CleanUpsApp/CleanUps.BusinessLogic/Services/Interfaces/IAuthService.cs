using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for handling authentication-related operations like login and password reset.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user based on provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>A Result containing the logged-in user information if successful, or an error message if authentication fails.</returns>
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);

        /// <summary>
        /// Initiates the password reset process for a user based on their email.
        /// Generates a token, stores it, and (conceptually) sends an email.
        /// </summary>
        /// <param name="request">Request containing the user's email.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Task<Result<bool>> RequestPasswordResetAsync(RequestPasswordResetRequest request);

        /// <summary>
        /// Validates a password reset token.
        /// </summary>
        /// <param name="request">Request containing the token string.</param>
        /// <returns>A Result indicating if the token is valid.</returns>
        Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request);

        /// <summary>
        /// Resets the user's password using a valid token.
        /// </summary>
        /// <param name="request">Request containing the token and new password.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Task<Result<bool>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}