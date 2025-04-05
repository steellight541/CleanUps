using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net.Mail;

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

            // Validate Password
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return Result<bool>.BadRequest("Password is required.");
            }

            // Basic password strength check
            if (dto.Password.Length < 8)
            {
                return Result<bool>.BadRequest("Password must be at least 8 characters long.");
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
            if (dto == null)
            {
                return Result<bool>.BadRequest("UpdateUserRequest cannot be null.");
            }

            // Validate UserId
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
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<bool>.BadRequest("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<bool>.BadRequest("Email is required.");
            }

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
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}