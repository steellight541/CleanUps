using CleanUps.Shared.DTOs.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    internal interface IUserValidator : IValidator<CreateUserRequest, UpdateUserRequest>;
}
