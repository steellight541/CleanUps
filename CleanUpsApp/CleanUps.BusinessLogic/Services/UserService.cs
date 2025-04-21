using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Helpers;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Provides business logic operations for managing users within the CleanUps application.
    /// Includes functionality for creating, retrieving, updating, deleting users, and changing passwords.
    /// Handles validation, data conversion, password hashing, and coordinates repository interactions.
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserValidator _validator;
        private readonly IUserConverter _converter;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="repository">Repository for user data access.</param>
        /// <param name="validator">Validator for user DTOs.</param>
        /// <param name="converter">Converter for User models and DTOs.</param>
        /// <param name="emailService">Service for sending user-related emails (welcome, deletion, etc.).</param>
        public UserService(IUserRepository repository,
                           IUserValidator validator,
                           IUserConverter converter,
                           IEmailService emailService)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
            _emailService = emailService;
        }

        /// <summary>
        /// Retrieves all non-deleted users from the repository.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UserResponse"/> DTOs, or an error.</returns>
        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            // Step 1: Call repository to retrieve all users.
            Result<List<User>> repoResult = await _repository.GetAllAsync();

            // Step 2: Convert domain model users to response DTOs and return.
            return repoResult.Transform(users => _converter.ToResponseList(users));
        }

        /// <summary>
        /// Retrieves a single non-deleted user by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="UserResponse"/> DTO if found, or an error (BadRequest, NotFound).</returns>
        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            // Step 1: Validate the user ID.
            var validationResult = _validator.ValidateId(id);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }
            // Step 3: Call repository to retrieve the user by ID.
            Result<User> repoResult = await _repository.GetByIdAsync(id);

            // Step 4: Convert domain model user to response DTO and return.
            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Creates a new user in the system.
        /// Validates the input, hashes the password, saves the user via the repository,
        /// and triggers sending a welcome email.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreateUserRequest"/> DTO containing the new user's data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="UserResponse"/> DTO, or an error (BadRequest, Conflict).</returns>
        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest createRequest)
        {
            // Step 1: Validate the create request DTO.
            var validationResult = _validator.ValidateForCreate(createRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert request DTO to a domain model.
            User userModel = _converter.ToModel(createRequest);

            // Step 4: Hash the user's password for secure storage.
            userModel.PasswordHash = PasswordHelper.HashPassword(createRequest.Password);

            // Step 5: Call repository to create the user.
            var repoResult = await _repository.CreateAsync(userModel);

            // Step 6: If user creation was successful, attempt to send welcome email.
            if(repoResult.IsSuccess)
            {
                try
                {
                    // Step 7: Send welcome email to the new user.
                    await _emailService.SendWelcomeEmailAsync(userModel.Email, userModel.Name);
                }
                catch (Exception ex)
                {
                    // Step 8: Log email failure but don't fail the user creation.
                    Console.WriteLine($"Error sending welcome email: {ex.Message}");
                }
            }

            // Step 9: Convert domain model to response DTO and return.
            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Updates an existing user's information (Name, Email, RoleId).
        /// Password updates should be handled via <see cref="ChangePasswordAsync"/> or the password reset flow.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateUserRequest"/> DTO containing the updated user data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="UserResponse"/> DTO, or an error (BadRequest, NotFound, Conflict).</returns>
        public async Task<Result<UserResponse>> UpdateAsync(UpdateUserRequest updateRequest)
        {
            // Step 1: Validate the update request DTO.
            var validationResult = _validator.ValidateForUpdate(updateRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert request DTO to a domain model.
            // Note: Conversion only maps fields present in UpdateUserRequest (UserId, Name, Email)
            User userModel = _converter.ToModel(updateRequest);

            // Step 4: Call repository to update the user.
            // Repository will handle preserving fields not being updated, like RoleId, PasswordHash, etc.
            var repoResult = await _repository.UpdateAsync(userModel);

            // Step 5: Convert domain model to response DTO and return.
            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Soft-deletes a user by marking them as deleted in the repository.
        /// Triggers sending a deletion confirmation email.
        /// </summary>
        /// <param name="deleteRequest">The <see cref="DeleteUserRequest"/> containing the ID of the user to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="UserResponse"/> DTO of the (now deleted) user, or an error (BadRequest, NotFound).</returns>
        public async Task<Result<UserResponse>> DeleteAsync(DeleteUserRequest deleteRequest)
        {
            // Step 1: Validate the user ID to delete.
            var validationResult = _validator.ValidateId(deleteRequest.Id);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to soft-delete the user.
            Result<User> repoResult = await _repository.DeleteAsync(deleteRequest.Id);

            // Step 4: If deletion was successful, attempt to send deletion email.
            if(repoResult.IsSuccess)
            {
                try
                {
                    // Step 5: Send deleted confirmation email to the user.
                    await _emailService.SendDeletedEmailAsync(repoResult.Data.Email, repoResult.Data.Name);
                }
                catch (Exception ex)
                {
                    // Step 6: Log email failure but don't fail the deletion operation.
                    Console.WriteLine($"Error sending deleted email: {ex.Message}");
                }
            }

            // Step 7: Convert domain model to response DTO and return.
            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Changes the password for a specified user.
        /// Validates input, fetches user details, hashes the new password, updates the repository,
        /// and triggers sending a password change confirmation email.
        /// </summary>
        /// <param name="changeRequest">The <see cref="ChangePasswordRequest"/> containing the user ID and new password.</param>
        /// <returns>A <see cref="Result{Boolean}"/> indicating success or failure.</returns>
        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest changeRequest)
        {
            // Step 1: Validate the password change request.
            var validationResult = _validator.ValidateForPasswordChange(changeRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<bool>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Retrieve the user by ID to ensure they exist and to get their email for confirmation.
            var userResult = await _repository.GetByIdAsync(changeRequest.UserId);

            // Step 4: If user not found, return NotFound error.
            if (!userResult.IsSuccess)
            {
                return Result<bool>.NotFound($"User with ID {changeRequest.UserId} not found.");
            }

            // Step 5: Store user details for later use in email confirmation.
            User user = userResult.Data;

            // Step 6: Hash the new password for secure storage.
            string newPasswordHash = PasswordHelper.HashPassword(changeRequest.NewPassword);

            // Step 7: Call repository to update the user's password.
            var updateResult = await _repository.UpdatePasswordAsync(changeRequest.UserId, newPasswordHash);
            
            // Step 8: If update fails, return the failure result.
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }

            // Step 9: Attempt to send password reset confirmation email.
            try
            {
                 await _emailService.SendPasswordResetConfirmationEmailAsync(user.Email, user.Name);
            }
            catch (Exception ex)
            {
                // Step 10: Log email failure but don't fail the password change operation.
                Console.WriteLine($"Error sending password change confirmation email: {ex.Message}");
            }

            // Step 11: Return the success result from the password update.
            return updateResult;
        }
    }
}
