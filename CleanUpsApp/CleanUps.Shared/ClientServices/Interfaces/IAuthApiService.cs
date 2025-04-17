using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    /// <summary>
    /// Interface for client-side authentication and password reset operations.
    /// </summary>
    public interface IAuthApiService
    {
        // Potentially add LoginAsync signature if needed
        // Task<Result<LoginResponse>> LoginAsync(LoginRequest request);

        /// <summary>
        /// Sends a request to the API to initiate password reset.
        /// Returns the confirmation message from the API.
        /// </summary>
        Task<Result<string>> RequestPasswordResetAsync(RequestPasswordResetRequest request);

        /// <summary>
        /// Sends a request to the API to validate a password reset token.
        /// </summary>
        Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request);

        /// <summary>
        /// Sends a request to the API to reset the password using a token.
        /// Returns the confirmation message from the API.
        /// </summary>
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
