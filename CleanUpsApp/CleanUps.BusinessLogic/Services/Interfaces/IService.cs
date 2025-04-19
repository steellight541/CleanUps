using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Generic service interface for CRUD operations on business entities.
    /// </summary>
    /// <typeparam name="TResponse">The response DTO type returned to clients.</typeparam>
    /// <typeparam name="TCreateRequest">The request DTO type used for creation operations.</typeparam>
    /// <typeparam name="TUpdateRequest">The request DTO type used for update operations.</typeparam>
    /// <typeparam name="TDeleteRequest">The request DTO type used for delete operations.</typeparam>
    public interface IService<TResponse, TCreateRequest, TUpdateRequest, TDeleteRequest>
        where TResponse : Response
        where TCreateRequest : CreateRequest
        where TUpdateRequest : UpdateRequest
        where TDeleteRequest : DeleteRequest
    {
        /// <summary>
        /// Retrieves all entities of the specified type.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <typeparamref name="TResponse"/> objects if successful, or an error message if the operation fails.</returns>
        Task<Result<List<TResponse>>> GetAllAsync();

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <typeparamref name="TResponse"/> if found, or an error message if the operation fails.</returns>
        Task<Result<TResponse>> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new entity based on the provided data.
        /// </summary>
        /// <param name="entity">The data for creating the new entity.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <typeparamref name="TResponse"/> if successful, or an error message if the operation fails.</returns>
        Task<Result<TResponse>> CreateAsync(TCreateRequest entity);

        /// <summary>
        /// Updates an existing entity based on the provided data.
        /// </summary>
        /// <param name="entity">The data for updating the entity.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <typeparamref name="TResponse"/> if successful, or an error message if the operation fails.</returns>
        Task<Result<TResponse>> UpdateAsync(TUpdateRequest entity);

        /// <summary>
        /// Deletes an entity based on the provided data.
        /// </summary>
        /// <param name="entity">The data identifying the entity to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted <typeparamref name="TResponse"/> if successful, or an error message if the operation fails.</returns>
        Task<Result<TResponse>> DeleteAsync(TDeleteRequest entity);
    }
}
