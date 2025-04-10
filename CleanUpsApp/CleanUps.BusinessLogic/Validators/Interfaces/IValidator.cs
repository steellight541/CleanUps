using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Validators.Interfaces
{
    /// <summary>
    /// Generic validator interface for validating data transfer objects.
    /// Provides methods for validating creation, update, and ID validation.
    /// </summary>
    /// <typeparam name="CreateDto">The create request DTO type.</typeparam>
    /// <typeparam name="UpdateDto">The update request DTO type.</typeparam>
    internal interface IValidator<CreateDto, UpdateDto> 
        where CreateDto : CreateRequest
        where UpdateDto : UpdateRequest
    {
        /// <summary>
        /// Validates a data transfer object for creating a new entity.
        /// </summary>
        /// <param name="dto">The create request DTO to validate.</param>
        /// <returns>A <see cref="Result{T}"/> containing a boolean indicating validation success or an error message if validation fails.</returns>
        Result<bool> ValidateForCreate(CreateDto dto);
        
        /// <summary>
        /// Validates a data transfer object for updating an existing entity.
        /// </summary>
        /// <param name="dto">The update request DTO to validate.</param>
        /// <returns>A <see cref="Result{T}"/> containing a boolean indicating validation success or an error message if validation fails.</returns>
        Result<bool> ValidateForUpdate(UpdateDto dto);
        
        /// <summary>
        /// Validates an entity ID.
        /// </summary>
        /// <param name="id">The ID to validate.</param>
        /// <returns>A <see cref="Result{T}"/> containing a boolean indicating validation success or an error message if validation fails.</returns>
        Result<bool> ValidateId(int id);
    }
}
