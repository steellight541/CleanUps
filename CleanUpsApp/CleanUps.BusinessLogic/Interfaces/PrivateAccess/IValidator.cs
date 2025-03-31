using CleanUps.Shared.DTOs.Flags;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IValidator<T> where T : RecordDTO
    {
        OperationResult<T> ValidateForCreate(T dto);

        OperationResult<T> ValidateForUpdate(int id, T dto);

        OperationResult<string> ValidateId(int id);
    }
}
