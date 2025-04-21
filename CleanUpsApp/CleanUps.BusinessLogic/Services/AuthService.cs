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
            // Step 1: Validate the login request DTO.
            var validationResult = _validator.ValidateForLogin(loginRequest);
            if (!validationResult.IsSuccess)
            {
                // Step 2: Return BadRequest if validation fails.
                return Result<LoginResponse>.BadRequest(validationResult.ErrorMessage ?? "Invalid login request.");
            }

            // Step 3: Retrieve the user by email from the repository.
            var userResult = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (!userResult.IsSuccess)
            {
                // Step 4: Return Unauthorized using a generic message if user not found (security).
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }
            User user = userResult.Data;

            // Step 5: Verify the provided password against the user's stored hash.
            if (!PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                // Step 6: Return Unauthorized using a generic message if password doesn't match (security).
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }

            // Step 7: Create the LoginResponse DTO upon successful authentication.
            LoginResponse loginResponse = new LoginResponse(
                user.UserId,
                user.Name,
                user.Email,
                user.Role is not null ? (RoleDTO)user.Role.Id : RoleDTO.Volunteer // Map Role or default
            );

            // Step 8: Return successful result with login response data.
            return Result<LoginResponse>.Ok(loginResponse);
        }

        /// <summary>
        /// Initiates the password reset process for a user identified by their email address.
        /// Generates a secure, time-limited token, stores it, and triggers sending a password reset email.
        /// Does not reveal whether the email exists to prevent user enumeration attacks.
        /// </summary>
        /// <param name="request">The <see cref="EmailPasswordResetRequest"/> containing the user's email.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> of <see cref="bool"/>. Always returns <c>Ok(true)</c> to the caller unless input validation fails,
        /// to avoid revealing if an email exists in the system. Internal errors are logged.
        /// </returns>
        public async Task<Result<bool>> RequestPasswordResetAsync(EmailPasswordResetRequest request)
        {
            // Step 1: Validate the password reset request DTO.
            var validationResult = _validator.ValidateForPasswordResetRequest(request);
            if (!validationResult.IsSuccess)
            {
                // Step 2: Return failure if validation fails.
                return validationResult;
            }

            // Step 3: Find the user by email.
            var userResult = await _userRepository.GetByEmailAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                // Step 4: If user not found, log info and return success to prevent email enumeration.
                _logger.LogInformation("Password reset requested for non-existent or invalid email: {Email}", request.Email);
                return Result<bool>.Ok(true); // Pretend success for security
            }
            User user = userResult.Data;

            // Step 5: Generate a secure password reset token.
            string tokenString = GenerateSecureToken();

            // Step 6: Create a PasswordResetToken record with user ID, token, and expiration.
            PasswordResetToken tokenRecord = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = tokenString,
                ExpirationDate = DateTime.UtcNow.AddMinutes(15), // Set token expiry (15 minutes)
                IsUsed = false
            };

            // Step 7: Save the generated token to the database.
            var createTokenResult = await _tokenRepository.CreateAsync(tokenRecord);
            if (!createTokenResult.IsSuccess)
            {
                // Step 8: Log error if token saving fails, but still return success to the user.
                _logger.LogError("Failed to save password reset token for user {UserId}: {Error}", user.UserId, createTokenResult.ErrorMessage);
                return Result<bool>.Ok(true); // Pretend success for security
            }

            // Step 9: Attempt to send the password reset email containing the token.
            try
            {
                await _emailService.SendPasswordResetEmailAsync(user.Email, user.Name, tokenString);
            }
            catch (Exception ex)
            {
                // Step 10: Log error if email sending fails, but don't block the success response.
                _logger.LogError(ex, "Failed to send password reset email to {Email} for user {UserId}", user.Email, user.UserId);
            }

            // Step 11: Return success to the caller (hiding potential backend issues).
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
            // Step 1: Validate the token validation request DTO.
            var validationResult = _validator.ValidateForTokenValidation(request);
            if (!validationResult.IsSuccess)
            {
                // Step 2: Return failure if validation fails.
                return validationResult;
            }

            // Step 3: Attempt to retrieve the token using the repository (which performs checks).
            var tokenResult = await _tokenRepository.GetByTokenAsync(request.Token);

            if (!tokenResult.IsSuccess)
            {
                // Step 4: If token retrieval/validation fails, map repository error to specific failure result.
                switch(tokenResult.StatusCode)
                {
                    case 400: return Result<bool>.BadRequest(tokenResult.ErrorMessage ?? "Invalid token format.");
                    case 404: return Result<bool>.NotFound(tokenResult.ErrorMessage ?? "Token not found.");
                    case 409: return Result<bool>.Conflict(tokenResult.ErrorMessage ?? "Token already used or expired.");
                    default: return Result<bool>.InternalServerError(tokenResult.ErrorMessage ?? "Failed to validate token.");
                }
            }

            // Step 5: If tokenResult is successful, the token is valid.
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
            // Step 1: Validate the password reset DTO (token, new password, confirmation).
            var validationResult = _validator.ValidateForPasswordReset(request);
            if (!validationResult.IsSuccess)
            {
                // Step 2: Return failure if validation fails.
                return validationResult;
            }

            // Step 3: Re-validate the token exists, is not used, and not expired via the repository.
            var tokenResult = await _tokenRepository.GetByTokenAsync(request.Token);
             if (!tokenResult.IsSuccess)
            {
                 // Step 4: Map repository errors to specific failure results if token is invalid.
                switch(tokenResult.StatusCode)
                {
                    case 400: return Result<bool>.BadRequest(tokenResult.ErrorMessage ?? "Invalid token format.");
                    case 404: return Result<bool>.NotFound(tokenResult.ErrorMessage ?? "Token not found.");
                    case 409: return Result<bool>.Conflict(tokenResult.ErrorMessage ?? "Token already used or expired.");
                    default: return Result<bool>.InternalServerError(tokenResult.ErrorMessage ?? "Failed to validate token.");
                }
            }
            PasswordResetToken validToken = tokenResult.Data;

            // Step 5: Extract the User ID from the validated token.
            int userId = validToken.UserId;

            // Step 6: Hash the user's new password securely.
            string newPasswordHash = PasswordHelper.HashPassword(request.NewPassword);

            // Step 7: Update the user's password hash in the user repository.
            var updatePasswordResult = await _userRepository.UpdatePasswordAsync(userId, newPasswordHash);
            if (!updatePasswordResult.IsSuccess)
            {
                // Step 8: Log error and return InternalServerError if password update fails.
                _logger.LogError("Failed to update password for user {UserId} during reset: {Error}", userId, updatePasswordResult.ErrorMessage);
                return Result<bool>.InternalServerError("Failed to update password.");
            }

            // Step 9: Mark the password reset token as used in the token repository.
            var markUsedResult = await _tokenRepository.MarkAsUsedAsync(validToken);
            if (!markUsedResult.IsSuccess)
            {
                // Step 10: Log error if marking token as used fails (but continue, password was reset).
                 _logger.LogError("Failed to mark reset token {TokenId} as used for user {UserId}: {Error}", validToken.Id, userId, markUsedResult.ErrorMessage);
            }

            // Step 11: Attempt to retrieve user details to send a confirmation email.
            var userResult = await _userRepository.GetByIdAsync(userId);
            if (userResult.IsSuccess)
            {
                // Step 12: Send password reset confirmation email.
                try
                {
                    await _emailService.SendPasswordResetConfirmationEmailAsync(userResult.Data.Email, userResult.Data.Name);
                }
                catch (Exception ex)
                {
                    // Step 13: Log error if confirmation email fails, but don't fail the operation.
                    _logger.LogError(ex, "Failed to send password reset confirmation email to {Email} for user {UserId}", userResult.Data.Email, userId);
                }
            }
            else
            {
                 // Step 14: Log warning if user details couldn't be retrieved for confirmation email.
                 _logger.LogWarning("Could not retrieve user details for user {UserId} to send confirmation email.", userId);
            }

            // Step 15: Return success indicating password reset was successful.
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Generates a cryptographically secure random alphanumeric token string.
        /// Uses <see cref="RandomNumberGenerator"/> for security.
        /// </summary>
        /// <param name="length">The desired length of the token string (default is 8 characters).</param>
        /// <returns>An alphanumeric random token string (A-Z, a-z, 0-9).</returns>
        private string GenerateSecureToken(int length = 8)
        {
            // Step 1: Define the pool of allowed characters for the token.
            const string allowedTokenCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            
            // Step 2: Create a character array to build the resulting token.
            char[] tokenCharacters = new char[length];
            
            // Step 3: Create a byte array to hold the generated random bytes.
            byte[] randomBytes = new byte[length];
            
            // Step 4: Use a cryptographic random number generator for security.
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                // Step 5: Fill the byte array with random bytes.
                randomNumberGenerator.GetBytes(randomBytes);
            }
            
            // Step 6: Convert each random byte into a character from the allowed set.
            for (int i = 0; i < length; i++)
            {
                /*Detailed Explanation for this line:
                1.randomBytes[i]: Get the random byte generated for this spot in the token.
                   Think of this byte as a random number between 0 and 255.


                2.allowedTokenCharacters.Length: Count how many different characters we're
                   allowed to use(e.g., 62 if we use A-Z, a - z, 0 - 9).
                
                3.Pick a valid position: We need to use the random number(0 - 255)
                    to fairly choose one of the allowed characters(e.g., one of the 62).
                    The `%` symbol here is a programming trick that takes the random number
                    and effectively "wraps it around" the count of allowed characters.
                    This guarantees we get a position number that's definitely within the
                    range of our allowed characters(e.g., a number from 0 to 61).


                4.allowedTokenCharacters[position_number]: Use the position number
                    we just calculated to look up the character at that spot in our
                    allowedTokenCharacters string.

                5.tokenCharacters[i] = ... : Put the chosen character into the
                    current spot `i` of the token we are building.*/
                tokenCharacters[i] = allowedTokenCharacters[randomBytes[i] % allowedTokenCharacters.Length];
            }
            
            // Step 7: Convert the character array to a string and return it.
            return new string(tokenCharacters);
        }
    }
}