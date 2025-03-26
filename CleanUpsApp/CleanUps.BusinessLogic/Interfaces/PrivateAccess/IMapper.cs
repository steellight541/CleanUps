using CleanUps.BusinessDomain.Models.Flags;
using CleanUps.Shared.DTOs.Flags;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CleanUps.Configuration")]
[assembly: InternalsVisibleTo("CleanUps.Test")]
namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    /// <summary>
    /// Provides methods to map between domain models and data transfer objects (DTOs) 
    /// for types <typeparamref name="ModelClass"/> and <typeparamref name="DTORecord"/>.
    /// </summary>
    /// <typeparam name="ModelClass">
    /// The domain model type that must inherit from <see cref="EFModel"/>.
    /// </typeparam>
    /// <typeparam name="DTORecord">
    /// The DTO type that must inherit from <see cref="RecordDTO"/>.
    /// </typeparam>
    internal interface IMapper<ModelClass, DTORecord> where ModelClass : EFModel where DTORecord : RecordDTO
    {
        /// <summary>
        /// Converts a DTO of type <typeparamref name="DTORecord"/> to a domain model of type <typeparamref name="ModelClass"/>.
        /// </summary>
        /// <param name="dto">
        /// The DTO to convert.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="ModelClass"/> representing the converted DTO.
        /// </returns>
        ModelClass ConvertToModel(DTORecord dto);

        /// <summary>
        /// Converts a domain model of type <typeparamref name="ModelClass"/> to a DTO of type <typeparamref name="DTORecord"/>.
        /// </summary>
        /// <param name="model">
        /// The domain model to convert.
        /// </param>
        /// <returns>
        /// An instance of <typeparamref name="DTORecord"/> representing the converted model.
        /// </returns>
        DTORecord ConvertToDTO(ModelClass model);

        /// <summary>
        /// Converts a list of domain models to a list of DTOs.
        /// </summary>
        /// <param name="listOfModels">
        /// The list of domain models to convert.
        /// </param>
        /// <returns>
        /// A list of <typeparamref name="DTORecord"/> objects representing the converted models.
        /// </returns>
        List<DTORecord> ConvertToDTOList(List<ModelClass> listOfModels);

        /// <summary>
        /// Converts a list of DTOs to a list of domain models.
        /// </summary>
        /// <param name="listOfDTOs">
        /// The list of DTOs to convert.
        /// </param>
        /// <returns>
        /// A list of <typeparamref name="ModelClass"/> objects representing the converted DTOs.
        /// </returns>
        List<ModelClass> ConvertToModelList(List<DTORecord> listOfDTOs);

    }

}
