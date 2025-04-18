using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.DTOs.Users; // Added for LoginRequest
using CleanUps.Shared.ErrorHandling;
using System.Net.Mail; // For email validation

namespace CleanUps.BusinessLogic.Validators
{
    /// <summary>
    /// Provides validation logic for authentication-related data transfer objects (DTOs).
    /// Implements the <see cref="IAuthValidator"/> interface.
    /// </summary>
    internal class AuthValidator : IAuthValidator
    {
        /// <summary>
        /// Validates the request DTO for initiating a password reset.
        /// </summary>
        /// <param name="dto">The password reset request DTO.</param>
        /// <returns>A <see cref="Result{T}"/> indicating success or failure with error details.</returns>
        public Result<bool> ValidateForPasswordResetRequest(RequestPasswordResetRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("Request cannot be null.");
            }

            // Ensure the email field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<bool>.BadRequest("Email is required.");
            }

            // Verify that the email format is valid using the email validation helper method
            if (!IsValidEmail(dto.Email))
            {
                return Result<bool>.BadRequest("Invalid email format.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates the request DTO for validating a password reset token.
        /// </summary>
        /// <param name="dto">The token validation request DTO.</param>
        /// <returns>A <see cref="Result{T}"/> indicating success or failure with error details.</returns>
        public Result<bool> ValidateForTokenValidation(ValidateTokenRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("Request cannot be null.");
            }

            // Ensure the token field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                return Result<bool>.BadRequest("Token is required.");
            }

            // Add checks for token length or format if applicable
            // Example: if (dto.Token.Length != 64) return Result<bool>.BadRequest("Invalid token format.");

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates the request DTO for resetting a user's password using a token.
        /// </summary>
        /// <param name="dto">The password reset request DTO.</param>
        /// <returns>A <see cref="Result{T}"/> indicating success or failure with error details.</returns>
        public Result<bool> ValidateForPasswordReset(ResetPasswordRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("Request cannot be null.");
            }

            // Ensure the token field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                return Result<bool>.BadRequest("Token is required.");
            }

            // Ensure the new password field is provided and not empty
             if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return Result<bool>.BadRequest("New Password is required.");
            }

            // Ensure the confirm password field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return Result<bool>.BadRequest("Confirm Password is required.");
            }

            // Verify that both password fields match to prevent accidental typos
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return Result<bool>.BadRequest("Passwords do not match.");
            }

            // Validate password length for security and database constraints
            if (dto.NewPassword.Length < 8 || dto.NewPassword.Length > 50)
            {
                return Result<bool>.BadRequest("Password must be between 8 and 50 characters long.");
            }
            // Add more complexity rules if needed

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates the request DTO for user login.
        /// </summary>
        /// <param name="dto">The login request DTO.</param>
        /// <returns>A <see cref="Result{T}"/> indicating success or failure with error details.</returns>
        public Result<bool> ValidateForLogin(LoginRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("Login request cannot be null.");
            }

            // Ensure the email field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<bool>.BadRequest("Email is required.");
            }

            // Verify that the email format is valid using the email validation helper method
            if (!IsValidEmail(dto.Email))
            {
                 return Result<bool>.BadRequest("Invalid email format.");
            }

            // Ensure the password field is provided and not empty
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return Result<bool>.BadRequest("Password is required.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Performs a basic validation of an email address format.
        /// </summary>
        /// <param name="email">The email address string to validate.</param>
        /// <returns><c>true</c> if the email format is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidEmail(string email)
        {
            // Check for null or empty email string
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                // Use .NET's MailAddress class to verify the email format
                var addr = new MailAddress(email);
                // Ensure the parsed address exactly matches the input to catch partial matches
                return addr.Address == email;
            }
            catch
            {
                // Return false if the email couldn't be parsed as a valid address
                return false;
            }
        }
    }
} 