using CleanUps.Shared.DTOs.Users;

namespace CleanUps.Shared.ClientServices.Interfaces
{
    public interface IUserApiService : IApiService<UserResponse, CreateUserRequest, UpdateUserRequest, DeleteUserRequest>
    {
    }
}
