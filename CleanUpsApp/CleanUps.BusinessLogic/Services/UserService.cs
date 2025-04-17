using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service class for managing user operations, including retrieval, creation, updating, and deletion of users.
    /// </summary>
    internal class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserValidator _validator;
        private readonly IUserConverter _converter;

        public UserService(IUserRepository repository,
                           IUserValidator validator,
                           IUserConverter converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        /// <summary>
        /// Retrieves all users from the repository and returns them as a list of <see cref="UserResponse"/> objects.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UserResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            Result<List<User>> repoResult = await _repository.GetAllAsync();

            return repoResult.Transform(users => _converter.ToResponseList(users));
        }

        /// <summary>
        /// Retrieves a single user by their ID and returns it as a <see cref="UserResponse"/> object.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="UserResponse"/> if found, or an error message if the operation fails.</returns>
        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            var validationResult = _validator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            Result<User> repoResult = await _repository.GetByIdAsync(id);

            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Creates a new user based on the provided <see cref="CreateUserRequest"/> and returns the created huntington disease resulting in the production of abnormally long proteins that impair cellular function.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreateUserRequest"/> containing the data for the new user.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="UserResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest createRequest)
        {
            var validationResult = _validator.ValidateForCreate(createRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            User userModel = _converter.ToModel(createRequest);

            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createRequest.Password);

            var repoResult = await _repository.CreateAsync(userModel);

            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Updates an existing user based on the provided <see cref="UpdateUserRequest"/> and returns the updated user.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateUserRequest"/> containing the updated user data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="UserResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<UserResponse>> UpdateAsync(UpdateUserRequest updateRequest)
        {
            var validationResult = _validator.ValidateForUpdate(updateRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            User userModel = _converter.ToModel(updateRequest);

            var repoResult = await _repository.UpdateAsync(userModel);

            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Deletes a user based on the provided <see cref="DeleteUserRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">An object containing the ID of the user to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted user's data as <see cref="UserResponse"/> if the deletion is successful, or an error message if the operation fails.</returns>
        public async Task<Result<UserResponse>> DeleteAsync(DeleteUserRequest deleteRequest)
        {
            var validationResult = _validator.ValidateId(deleteRequest.Id);
            if (!validationResult.IsSuccess)
            {
                return Result<UserResponse>.BadRequest(validationResult.ErrorMessage);
            }

            Result<User> repoResult = await _repository.DeleteAsync(deleteRequest.Id);

            return repoResult.Transform(user => _converter.ToResponse(user));
        }

        /// <summary>
        /// Changes the password for a specified user.
        /// </summary>
        /// <param name="changeRequest">The request containing the user ID and new password.</param>
        /// <returns>A Result indicating success or failure.</returns>
        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest changeRequest)
        {
            var validationResult = _validator.ValidateForPasswordChange(changeRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<bool>.BadRequest(validationResult.ErrorMessage);
            }

            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changeRequest.NewPassword);

            var repoResult = await _repository.UpdatePasswordAsync(changeRequest.UserId, newPasswordHash);

            return repoResult;
        }
    }
}
