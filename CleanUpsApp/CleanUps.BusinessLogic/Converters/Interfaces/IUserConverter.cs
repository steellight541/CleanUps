using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    internal interface IUserConverter : IConverter<User, UserResponse, CreateUserRequest, UpdateUserRequest>;
}
