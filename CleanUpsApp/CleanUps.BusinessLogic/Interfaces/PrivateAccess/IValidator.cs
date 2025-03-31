using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IValidator<T> where T : RecordDTO
    {
        void ValidateForCreate(T dto);

        void ValidateForUpdate(int id, T dto);

        void ValidateId(int id);
    }
}
