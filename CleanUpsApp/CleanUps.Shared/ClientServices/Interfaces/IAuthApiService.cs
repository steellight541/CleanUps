using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    /// <summary>
    /// Interface for client-side authentication and password management operations.
    /// Provides methods for user login, password reset requests, token validation,
    /// and password reset completion, with standardized error handling.
    /// </summary>
    public interface IAuthApiService
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>
        /// A Result containing the authenticated user information and access token if successful,
        /// or appropriate error information if authentication fails.
        /// </returns>
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);

        /// <summary>
        /// Initiates the password reset process for a user by sending a reset token to their email.
        /// </summary>
        /// <param name="request">The request containing the user's email address.</param>
        /// <returns>
        /// A Result containing a confirmation message if the request was processed successfully,
        /// or appropriate error information if the request fails.
        /// </returns>
        Task<Result<string>> RequestPasswordResetAsync(EmailPasswordResetRequest request);

        /// <summary>
        /// Validates a password reset token to ensure it's valid, not expired, and can be used.
        /// </summary>
        /// <param name="request">The request containing the token to validate.</param>
        /// <returns>
        /// A Result with a true value if the token is valid,
        /// or appropriate error information if the token is invalid, expired, or already used.
        /// </returns>
        Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request);

        /// <summary>
        /// Completes the password reset process by setting a new password using a valid token.
        /// </summary>
        /// <param name="request">The request containing the token, new password, and confirmation.</param>
        /// <returns>
        /// A Result containing a confirmation message if the password was reset successfully,
        /// or appropriate error information if the reset fails.
        /// </returns>
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
