using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IDTOValidator<DTORecord> where DTORecord : RecordFlag
    {
        public void ValidateForCreate(DTORecord dto);

        public void ValidateForUpdate(int id, DTORecord dto);

        public void ValidateId(int id);
    }
}
