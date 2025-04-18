using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CleanUps.BusinessLogic.Validators
{
    /// <summary>
    /// Validator for user-related operations, implementing validation rules for creating and updating users.
    /// </summary>
    internal class UserValidator : IUserValidator
    {

        /// <summary>
        /// Validates a CreateUserRequest before creating a new user.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="dto">The CreateUserRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForCreate(CreateUserRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("CreateUserRequest cannot be null.");
            }

            // Validate common fields (Name and Email)
            var commonValidation = ValidateNameAndEmail(dto.Name, dto.Email);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            // Ensure a password is provided for the new user account
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return Result<bool>.BadRequest("Password is required.");
            }

            // Validate password length to ensure it meets security requirements
            if (dto.Password.Length < 8 || dto.Password.Length > 50)
            {
                return Result<bool>.BadRequest("Password must be between 8 and 50 characters long.");
            }


            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates an UpdateUserRequest before updating an existing user.
        /// Ensures all required fields are present and properly formatted.
        /// </summary>
        /// <param name="dto">The UpdateUserRequest to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateForUpdate(UpdateUserRequest dto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (dto == null)
            {
                return Result<bool>.BadRequest("UpdateUserRequest cannot be null.");
            }

            // Ensure the user ID is valid (positive number)
            if (dto.UserId <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }

            // Validate common fields (Name and Email)
            var commonValidation = ValidateNameAndEmail(dto.Name, dto.Email);
            if (!commonValidation.IsSuccess)
            {
                return commonValidation;
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates a user ID to ensure it's a positive integer.
        /// </summary>
        /// <param name="id">The user ID to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        public Result<bool> ValidateId(int id)
        {
            // Ensure the user ID is valid (positive number)
            if (id <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }
            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Validates common fields (Name and Email) used in both create and update operations.
        /// </summary>
        /// <param name="name">The user's name to validate</param>
        /// <param name="email">The user's email to validate</param>
        /// <returns>A Result indicating success or failure with an error message</returns>
        private Result<bool> ValidateNameAndEmail(string name, string email)
        {
            // Ensure name field is provided and not empty
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<bool>.BadRequest("Name is required.");
            }
            
            // Validate name length to ensure it fits database constraints
             if (name.Length > 100)
            {
                return Result<bool>.BadRequest("Name cannot exceed 100 characters.");
            }

            // Ensure email field is provided and not empty
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<bool>.BadRequest("Email is required.");
            }

            // Validate email length to ensure it fits database constraints
            if (email.Length > 255)
            {
                return Result<bool>.BadRequest("Email cannot exceed 255 characters.");
            }

            // Verify that the email format is valid using the email validation helper method
            if (!IsValidEmail(email))
            {
                return Result<bool>.BadRequest("Invalid email format.");
            }

            return Result<bool>.Ok(true);
        }

        /// <summary>
        /// Checks if an email string is in a valid format.
        /// </summary>
        /// <param name="email">The email string to validate</param>
        /// <returns>True if the email is valid, false otherwise</returns>
        private bool IsValidEmail(string email)
        {
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

        /// <summary>
        /// Validates a ChangePasswordRequest before updating a user's password.
        /// Ensures UserId is valid and the new password meets complexity requirements.
        /// </summary>
        /// <param name="changePasswordRequestDto">The ChangePasswordRequest DTO to validate.</param>
        /// <returns>A Result indicating success or failure with an error message.</returns>
        public Result<bool> ValidateForPasswordChange(ChangePasswordRequest changePasswordRequestDto)
        {
            // Verify that the DTO itself is not null to prevent null reference exceptions
            if (changePasswordRequestDto == null)
            {
                return Result<bool>.BadRequest("ChangePasswordRequest cannot be null.");
            }

            // Ensure the user ID is valid (positive number)
            if (changePasswordRequestDto.UserId <= 0)
            {
                return Result<bool>.BadRequest("User Id must be greater than zero.");
            }

            // Ensure the new password is provided and not empty
            if (string.IsNullOrWhiteSpace(changePasswordRequestDto.NewPassword))
            {
                return Result<bool>.BadRequest("New Password is required.");
            }

            // Validate password length to ensure it meets security requirements
            if (changePasswordRequestDto.NewPassword.Length < 8 || changePasswordRequestDto.NewPassword.Length > 50)
            {
                return Result<bool>.BadRequest("New Password must be between 8 and 50 characters long.");
            }

            return Result<bool>.Ok(true);
        }
    }
}