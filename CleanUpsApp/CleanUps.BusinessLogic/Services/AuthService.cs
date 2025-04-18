using CleanUps.BusinessLogic.Helpers;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.ErrorHandling;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service responsible for handling user authentication, login, and password reset workflows.
    /// Coordinates interactions between repositories, validators, and email services for auth operations.
    /// </summary>
    internal class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly IAuthValidator _validator;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for accessing user data.</param>
        /// <param name="tokenRepository">Repository for managing password reset tokens.</param>
        /// <param name="emailService">Service for sending emails (password reset, confirmations).</param>
        /// <param name="validator">Validator for authentication-related DTOs.</param>
        /// <param name="logger">Logger for recording service operations and errors.</param>
        public AuthService(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository,
            IEmailService emailService,
            IAuthValidator validator,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _validator = validator;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user based on the provided email and password.
        /// Verifies credentials against stored user data and password hash.
        /// </summary>
        /// <param name="loginRequest">The login request DTO containing email and password.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> of <see cref="LoginResponse"/>. On success, contains the user's login details.
        /// On failure, contains an error status code and message (e.g., BadRequest, Unauthorized).
        /// </returns>
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            // Use validator
            var validationResult = _validator.ValidateForLogin(loginRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<LoginResponse>.BadRequest(validationResult.ErrorMessage ?? "Invalid login request.");
            }

            // Get the user by email
            var userResult = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (!userResult.IsSuccess)
            {
                // Use a generic message for security
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }
            User user = userResult.Data;

            // Verify the password
            if (!PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }

            // Create and return the login response
            LoginResponse loginResponse = new LoginResponse(
                user.UserId,
                user.Name,
                user.Email,
                user.Role is not null ? (RoleDTO)user.Role.Id : RoleDTO.Volunteer
            );

            return Result<LoginResponse>.Ok(loginResponse);
        }

        /// <summary>
        /// Initiates the password reset process for a user identified by their email address.
        /// Generates a secure, time-limited token, stores it, and triggers sending a password reset email.
        /// Does not reveal whether the email exists to prevent user enumeration attacks.
        /// </summary>
        /// <param name="request">The <see cref="RequestPasswordResetRequest"/> containing the user's email.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> of <see cref="bool"/>. Always returns <c>Ok(true)</c> to the caller unless input validation fails,
        /// to avoid revealing if an email exists in the system. Internal errors are logged.
        /// </returns>
        public async Task<Result<bool>> RequestPasswordResetAsync(RequestPasswordResetRequest request)
        {
            // Use validator
            var validationResult = _validator.ValidateForPasswordResetRequest(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // Find the user by email
            var userResult = await _userRepository.GetByEmailAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                // Don't reveal if the email exists or not for security, return success anyway
                _logger.LogInformation("Password reset requested for non-existent or invalid email: {Email}", request.Email);
                return Result<bool>.Ok(true); // Pretend success
            }
            User user = userResult.Data;

            // Generate a secure token
            string tokenString = GenerateSecureToken();

            // Create the token record
            PasswordResetToken tokenRecord = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = tokenString,
                ExpirationDate = DateTime.UtcNow.AddMinutes(30), // 30-minute expiry
                IsUsed = false
            };

            // Save the token to the database
            var createTokenResult = await _tokenRepository.CreateAsync(tokenRecord);
            if (!createTokenResult.IsSuccess)
            {
                _logger.LogError("Failed to save password reset token for user {UserId}: {Error}", user.UserId, createTokenResult.ErrorMessage);
                // Still return success to the caller, don't reveal backend issues
                return Result<bool>.Ok(true);
            }

            // ** Use Email Service **
            try
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, user.Name, tokenString);
            }
            catch (Exception ex)
            {
                // Log email sending failure but don't block the user-facing success response
                _logger.LogError(ex, "Failed to send password reset email to {Email} for user {UserId}", user.Email, user.UserId);
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates a provided password reset token.
        /// Checks if the token exists, has not expired, and has not already been used.
        /// </summary>
        /// <param name="request">The <see cref="ValidateTokenRequest"/> containing the token string.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> of <see cref="bool"/>. Returns <c>Ok(true)</c> if the token is valid.
        /// Returns <c>Failure</c> with a specific status code (BadRequest, NotFound, Conflict) if the token is invalid.
        /// </returns>
        public async Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request)
        {
            // Use validator
            var validationResult = _validator.ValidateForTokenValidation(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // Use the repository method which includes validation checks
            var tokenResult = await _tokenRepository.GetByTokenAsync(request.Token);

            if (!tokenResult.IsSuccess)
            {
                // Convert repository errors to appropriate failure types
                switch(tokenResult.StatusCode)
                {
                    case 400: return Result<bool>.BadRequest(tokenResult.ErrorMessage ?? "Invalid token format.");
                    case 404: return Result<bool>.NotFound(tokenResult.ErrorMessage ?? "Token not found.");
                    case 409: return Result<bool>.Conflict(tokenResult.ErrorMessage ?? "Token already used or expired.");
                    default: return Result<bool>.InternalServerError(tokenResult.ErrorMessage ?? "Failed to validate token.");
                }
            }

            // Token is valid, exists, not used, and not expired
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Resets a user's password using a validated token and the new password.
        /// Validates the token again, hashes the new password, updates the user record, marks the token as used,
        /// and attempts to send a password change confirmation email.
        /// </summary>
        /// <param name="request">The <see cref="ResetPasswordRequest"/> containing the token and new password details.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> of <see cref="bool"/>. Returns <c>Ok(true)</c> on successful password reset.
        /// Returns <c>Failure</c> with a specific status code if validation fails or an internal error occurs.
        /// </returns>
        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            // Step 1: Validate Input requestDTO
            var validationResult = _validator.ValidateForPasswordReset(request);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // Step 2: Validate the Token (If it exists, if it is not used, and if it is not expired)
            var tokenResult = await _tokenRepository.GetByTokenAsync(request.Token);
             if (!tokenResult.IsSuccess)
            {
                 // Map repository errors
                switch(tokenResult.StatusCode)
                {
                    case 400: return Result<bool>.BadRequest(tokenResult.ErrorMessage ?? "Invalid token format.");
                    case 404: return Result<bool>.NotFound(tokenResult.ErrorMessage ?? "Token not found.");
                    case 409: return Result<bool>.Conflict(tokenResult.ErrorMessage ?? "Token already used or expired.");
                    default: return Result<bool>.InternalServerError(tokenResult.ErrorMessage ?? "Failed to validate token.");
                }
            }
            PasswordResetToken validToken = tokenResult.Data;

            // Step 3: Get the associated User ID from the validated token
            int userId = validToken.UserId;

            // Step 4: Hash the New Password
            string newPasswordHash = PasswordHelper.HashPassword(request.NewPassword);

            // Step 5: Update User's Password in Repository
            var updatePasswordResult = await _userRepository.UpdatePasswordAsync(userId, newPasswordHash);
            if (!updatePasswordResult.IsSuccess)
            {
                _logger.LogError("Failed to update password for user {UserId} during reset: {Error}", userId, updatePasswordResult.ErrorMessage);
                return Result<bool>.InternalServerError("Failed to update password.");
            }

            // Step 6: Mark the Token as Used
            var markUsedResult = await _tokenRepository.MarkAsUsedAsync(validToken);
            if (!markUsedResult.IsSuccess)
            {
                // Log this failure, but proceed as password was reset successfully
                 _logger.LogError("Failed to mark reset token {TokenId} as used for user {UserId}: {Error}", validToken.Id, userId, markUsedResult.ErrorMessage);
            }

            // Step 7: Send Confirmation Email
            var userResult = await _userRepository.GetByIdAsync(userId);
            if (userResult.IsSuccess)
            {
                try
                {
                    await _emailService.SendPasswordResetConfirmationEmailAsync(userResult.Data.Email, userResult.Data.Name);
                }
                catch (Exception ex)
                {
                    // Log email sending failure but don't let it fail the overall operation
                    _logger.LogError(ex, "Failed to send password reset confirmation email to {Email} for user {UserId}", userResult.Data.Email, userId);
                }
            }
            else
            {
                 _logger.LogWarning("Could not retrieve user details for user {UserId} to send confirmation email.", userId);
            }

            // Step 8: Return Success
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Generates a cryptographically secure, URL-safe token string.
        /// Uses <see cref="RandomNumberGenerator"/> for security.
        /// </summary>
        /// <param name="length">The desired length of the raw byte array before Base64 encoding (default is 32, resulting in approx. 44 character string).</param>
        /// <returns>A URL-safe Base64 encoded string token.</returns>
        private string GenerateSecureToken(int length = 32)
        {
            // Using URL-safe base64 encoding
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] tokenBytes = new byte[length];
                rng.GetBytes(tokenBytes);
                // Replace URL-unsafe characters and remove padding
                return Convert.ToBase64String(tokenBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
            }
        }
    }
}