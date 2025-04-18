using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    public interface IUserApiService : IApiService<UserResponse, CreateUserRequest, UpdateUserRequest, DeleteUserRequest>
    {
        /// <summary>
        /// Sends a request to the API to change the user's password.
        /// </summary>
        /// <param name="changeRequest">The password change request details.</param>
        /// <returns>A Result indicating success or failure.</returns>
        Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest changeRequest);
    }
}
