using CleanUps.BusinessLogic.Helpers;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Enums;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service responsible for user authentication operations.
    /// Implements login functionality and user validation.
    /// </summary>
    internal class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the AuthService class.
        /// </summary>
        /// <param name="userRepository">The repository for user data access.</param>
        /// <param name="userConverter">The converter for transforming between User models and DTOs.</param>
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}