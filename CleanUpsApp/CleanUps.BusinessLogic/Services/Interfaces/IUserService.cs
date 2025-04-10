using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Service interface for managing user operations.
    /// Provides functionality for retrieving, creating, updating, and deleting users in the system.
    /// </summary>
    public interface IUserService : IService<UserResponse, CreateUserRequest, UpdateUserRequest, DeleteUserRequest>;
}
