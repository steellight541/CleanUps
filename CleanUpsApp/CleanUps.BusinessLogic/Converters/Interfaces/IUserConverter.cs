using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Users;

namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IUserConverter : IConverter<User, UserResponse, CreateUserRequest, UpdateUserRequest>;
}
