using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    /// <summary>
    /// Converter interface for converting between User model and User DTOs.
    /// Provides bidirectional conversion between User domain model and User-related DTOs.
    /// </summary>
    internal interface IUserConverter : IConverter<User, UserResponse, CreateUserRequest, UpdateUserRequest>;
}
