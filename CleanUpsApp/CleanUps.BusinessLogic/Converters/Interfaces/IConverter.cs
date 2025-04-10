using CleanUps.BusinessLogic.Models.AbstractModels;
using CleanUps.Shared.DTOs.AbstractDTOs;

namespace CleanUps.BusinessLogic.Converters.Interfaces
{
    /// <summary>
    /// Generic converter interface for converting between model classes and data transfer objects.
    /// Provides methods for bidirectional conversion between domain models and DTOs.
    /// </summary>
    /// <typeparam name="ModelClass">The entity model type derived from EntityFrameworkModel.</typeparam>
    /// <typeparam name="TResponse">The response DTO type returned to clients.</typeparam>
    /// <typeparam name="TCreateRequest">The request DTO type used for creation operations.</typeparam>
    /// <typeparam name="TUpdateRequest">The request DTO type used for update operations.</typeparam>
    internal interface IConverter<ModelClass, TResponse, TCreateRequest, TUpdateRequest>
        where ModelClass : EntityFrameworkModel
        where TResponse : Response
        where TCreateRequest : CreateRequest
        where TUpdateRequest : UpdateRequest
    {
        /// <summary>
        /// Converts a response DTO to a model entity.
        /// </summary>
        /// <param name="dto">The response DTO to convert.</param>
        /// <returns>A new or updated model entity.</returns>
        ModelClass ToModel(TResponse dto);
        
        /// <summary>
        /// Converts a create request DTO to a model entity.
        /// </summary>
        /// <param name="dto">The create request DTO to convert.</param>
        /// <returns>A new model entity.</returns>
        ModelClass ToModel(TCreateRequest dto);
        
        /// <summary>
        /// Converts an update request DTO to a model entity.
        /// </summary>
        /// <param name="dto">The update request DTO to convert.</param>
        /// <returns>An updated model entity.</returns>
        ModelClass ToModel(TUpdateRequest dto);

        /// <summary>
        /// Converts a model entity to a response DTO.
        /// </summary>
        /// <param name="model">The model entity to convert.</param>
        /// <returns>A response DTO representing the model.</returns>
        TResponse ToResponse(ModelClass model);
        
        /// <summary>
        /// Converts a model entity to a create request DTO.
        /// </summary>
        /// <param name="model">The model entity to convert.</param>
        /// <returns>A create request DTO representing the model.</returns>
        TCreateRequest ToCreateRequest(ModelClass model);
        
        /// <summary>
        /// Converts a model entity to an update request DTO.
        /// </summary>
        /// <param name="model">The model entity to convert.</param>
        /// <returns>An update request DTO representing the model.</returns>
        TUpdateRequest ToUpdateRequest(ModelClass model);

        /// <summary>
        /// Converts a list of model entities to a list of response DTOs.
        /// </summary>
        /// <param name="listOfModels">The list of model entities to convert.</param>
        /// <returns>A list of response DTOs.</returns>
        List<TResponse> ToResponseList(List<ModelClass> listOfModels);
        
        /// <summary>
        /// Converts a list of response DTOs to a list of model entities.
        /// </summary>
        /// <param name="listOfDTOs">The list of response DTOs to convert.</param>
        /// <returns>A list of model entities.</returns>
        List<ModelClass> ToModelList(List<TResponse> listOfDTOs);
    }
}
