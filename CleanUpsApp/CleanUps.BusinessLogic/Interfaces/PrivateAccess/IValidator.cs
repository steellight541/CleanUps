using CleanUps.Shared.DTOs.Flags;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]
namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IValidator<T> where T : RecordDTO
    {
        Result<T> ValidateForCreate(T dto);
        Result<T> ValidateForUpdate(T dto);
        Result<string> ValidateId(int id);
    }
}
