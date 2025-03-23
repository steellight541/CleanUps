using CleanUps.BusinessDomain.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
[assembly: InternalsVisibleTo("CleanUps.Configuration")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    /// <summary>
    /// Defines a generic repository interface for CRUD operations on entities of type <typeparamref name="ModelClass"/> in the CleanUps application.
    /// This interface provides asynchronous methods for creating, reading, updating, and deleting entities.
    /// </summary>
    /// <typeparam name="ModelClass">The type of entity managed by the repository.</typeparam>
    internal interface ICRUDRepository<ModelClass> where ModelClass : ModelFlag
    {
        /// <summary>
        /// Creates a new entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeCreated">The entity of type <typeparamref name="ModelClass"/> to be created.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task CreateAsync(ModelClass entityToBeCreated);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="ModelClass"/> asynchronously from the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all entities of type <typeparamref name="ModelClass"/>.</returns>
        public Task<List<ModelClass>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="ModelClass"/> by its ID asynchronously from the database.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the entity to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entity of type <typeparamref name="ModelClass"/> if found, or <see langword="null"/> if not found.</returns>
        public Task<ModelClass> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeUpdated">The entity of type <typeparamref name="ModelClass"/> with updated values to persist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UpdateAsync(ModelClass entityToBeUpdated);

        /// <summary>
        /// Deletes an entity asynchronously from the database by its ID.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteAsync(int id);
    }
}
