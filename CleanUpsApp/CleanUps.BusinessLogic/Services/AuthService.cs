using CleanUps.BusinessLogic.Helpers;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service responsible for user authentication operations.
    /// Implements login functionality and user validation.
    /// </summary>
    internal class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="userRepository">The repository for user data access.</param>
        /// <param name="tokenRepository">The repository for password reset token data access.</param>
        /// <param name="logger">The logger for logging operations.</param>
        public AuthService(
            IUserRepository userRepository,
            IPasswordResetTokenRepository tokenRepository,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user based on provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login request containing user credentials.</param>
        /// <returns>A Result containing the logged-in user information if successful, or an error message if authentication fails.</returns>
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return Result<LoginResponse>.BadRequest("Login request cannot be null");
            }

            if (string.IsNullOrWhiteSpace(loginRequest.Email))
            {
                return Result<LoginResponse>.BadRequest("Email is required");
            }

            if (string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return Result<LoginResponse>.BadRequest("Password is required");
            }

            // Get the user by email
            var userResult = await _userRepository.GetByEmailAsync(loginRequest.Email);

            if (!userResult.IsSuccess)
            {
                // Return NotFound for non-existent user, but use a generic message for security
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }

            var user = userResult.Data;

            // Verify the password
            if (!PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }

            // Create and return the login response
            var loginResponse = new LoginResponse(
                user.UserId,
                user.Name,
                user.Email,
                user.Role is not null ? (RoleDTO)user.Role.Id : RoleDTO.Volunteer
            );

            return Result<LoginResponse>.Ok(loginResponse);
        }

        public async Task<Result<bool>> RequestPasswordResetAsync(RequestPasswordResetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result<bool>.BadRequest("Email is required.");
            }

            // Find the user by email
            var userResult = await _userRepository.GetByEmailAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                // Don't reveal if the email exists or not for security, return success anyway
                _logger.LogInformation("Password reset requested for non-existent or invalid email: {Email}", request.Email);
                return Result<bool>.Ok(true); // Pretend success
            }

            var user = userResult.Data;

            // Generate a secure token
            var tokenString = GenerateSecureToken();

            // Create the token record
            var tokenRecord = new PasswordResetToken
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

            // ** Simulate sending email **
            _logger.LogWarning("--- Password Reset Email Simulation ---");
            _logger.LogWarning("To: {Email}", user.Email);
            _logger.LogWarning("Subject: Reset Your Password");
            _logger.LogWarning("Body: Use this token to reset your password: {Token}", tokenString);
            _logger.LogWarning("--- End Simulation ---");
            // TODO: Replace simulation with actual call to _emailService.SendPasswordResetEmailAsync(user.Email, tokenString);

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request)
        {
            // Use the repository method which includes validation checks
            var tokenResult = await _tokenRepository.GetByTokenAsync(request.Token);

            if (!tokenResult.IsSuccess)
            {
                // Map repository errors to appropriate failure types
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

        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            // 1. Validate the token again
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
            var validToken = tokenResult.Data;

            // 2. Validate passwords match (already done by DTO validation, but good practice)
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result<bool>.BadRequest("Passwords do not match.");
            }

            // 3. Basic password complexity check (can be enhanced in a validator)
            if (request.NewPassword.Length < 8)
            {
                 return Result<bool>.BadRequest("Password must be at least 8 characters long.");
            }

            // 4. Get the associated User ID from the validated token
            int userId = validToken.UserId;

            // 5. Hash the new password
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // 6. Update the user's password in the user repository
            var updatePasswordResult = await _userRepository.UpdatePasswordAsync(userId, newPasswordHash);
            if (!updatePasswordResult.IsSuccess)
            {
                _logger.LogError("Failed to update password for user {UserId} during reset: {Error}", userId, updatePasswordResult.ErrorMessage);
                return Result<bool>.InternalServerError("Failed to update password.");
            }

            // 7. Mark the token as used
            var markUsedResult = await _tokenRepository.MarkAsUsedAsync(validToken);
            if (!markUsedResult.IsSuccess)
            {
                // Log this failure, but proceed as password was reset successfully
                 _logger.LogError("Failed to mark reset token {TokenId} as used for user {UserId}: {Error}", validToken.Id, userId, markUsedResult.ErrorMessage);
            }

            // 8. (Optional) Send confirmation email
            // var user = await _userRepository.GetByIdAsync(userId); // Need user email
            // if (user.IsSuccess) _logger.LogWarning("--- Password Reset Confirmation Email Simulation ---");
            // TODO: Send confirmation email

            return Result<bool>.Ok(true);
        }

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