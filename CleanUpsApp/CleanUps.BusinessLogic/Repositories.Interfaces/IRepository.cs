using CleanUps.BusinessLogic.Models.AbstractModels;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations.
    /// Provides methods for basic CRUD operations on entity models.
    /// </summary>
    /// <typeparam name="TModel">The entity model type derived from EntityFrameworkModel.</typeparam>
    internal interface IRepository<TModel> where TModel : EntityFrameworkModel
    {
        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created entity if successful, or an error message if the operation fails.</returns>
        Task<Result<TModel>> CreateAsync(TModel entity);
        
        /// <summary>
        /// Retrieves all entities of the specified type from the database.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of entities if successful, or an error message if the operation fails.</returns>
        Task<Result<List<TModel>>> GetAllAsync();
        
        /// <summary>
        /// Retrieves a single entity by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the retrieved entity if found, or an error message if the operation fails.</returns>
        Task<Result<TModel>> GetByIdAsync(int id);
        
        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity with updated data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated entity if successful, or an error message if the operation fails.</returns>
        Task<Result<TModel>> UpdateAsync(TModel entity);
        
        /// <summary>
        /// Deletes an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted entity if successful, or an error message if the operation fails.</returns>
        Task<Result<TModel>> DeleteAsync(int id);
    }
}