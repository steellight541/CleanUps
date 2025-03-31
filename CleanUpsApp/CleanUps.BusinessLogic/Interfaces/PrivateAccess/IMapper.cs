using CleanUps.BusinessLogic.Models.Flags;
using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]
namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    internal interface IMapper<ModelClass, DTORecord> where ModelClass : EFModel where DTORecord : RecordDTO
    {
        ModelClass ConvertToModel(DTORecord dto);
        DTORecord ConvertToDTO(ModelClass model);
        List<DTORecord> ConvertToDTOList(List<ModelClass> listOfModels);
        List<ModelClass> ConvertToModelList(List<DTORecord> listOfDTOs);
    }
}
