using CleanUps.BusinessDomain.Models.Flags;
using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]
namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    internal interface IMapper<ModelClass, DTORecord> where ModelClass : ModelFlag where DTORecord : RecordFlag
    {
        public ModelClass ConvertToModel(DTORecord dto);

        public DTORecord ConvertToDTO(ModelClass model);

        public List<DTORecord> ConvertToDTOList(List<ModelClass> listOfModels);

        public List<ModelClass> ConvertToModelList(List<DTORecord> listOfDTOs);

    }

}
