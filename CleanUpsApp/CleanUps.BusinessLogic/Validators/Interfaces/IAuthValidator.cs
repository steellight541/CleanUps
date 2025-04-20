using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Validator interface for validating authentication-related requests.
    /// </summary>
    internal interface IAuthValidator
    {
        /// <summary>
        /// Validates a RequestPasswordResetRequest.
        /// </summary>
        /// <param name="dto">The RequestPasswordResetRequest DTO.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Result<bool> ValidateForPasswordResetRequest(EmailPasswordResetRequest dto);

        /// <summary>
        /// Validates a ValidateTokenRequest.
        /// </summary>
        /// <param name="dto">The ValidateTokenRequest DTO.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Result<bool> ValidateForTokenValidation(ValidateTokenRequest dto);

        /// <summary>
        /// Validates a ResetPasswordRequest.
        /// </summary>
        /// <param name="dto">The ResetPasswordRequest DTO.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Result<bool> ValidateForPasswordReset(ResetPasswordRequest dto);

        /// <summary>
        /// Validates a LoginRequest.
        /// </summary>
        /// <param name="dto">The LoginRequest DTO.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Result<bool> ValidateForLogin(LoginRequest dto);
    }
} 