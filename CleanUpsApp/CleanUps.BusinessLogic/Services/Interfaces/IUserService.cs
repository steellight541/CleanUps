using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    public interface IUserService : IService<UserResponse, CreateUserRequest, UpdateUserRequest, DeleteUserRequest>;
}
