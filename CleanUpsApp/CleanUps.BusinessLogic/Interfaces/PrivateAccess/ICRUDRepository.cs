using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("CleanUps.DataAccess")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{
    /// <summary>
    /// Defines a generic repository interface for CRUD operations on entities of type <typeparamref name="T"/> in the CleanUps application.
    /// This interface provides asynchronous methods for creating, reading, updating, and deleting entities.
    /// </summary>
    /// <typeparam name="T">The type of entity managed by the repository.</typeparam>
    public interface ICRUDRepository<T>
    {
        /// <summary>
        /// Creates a new entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeCreated">The entity of type <typeparamref name="T"/> to be created.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task CreateAsync(T entityToBeCreated);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> asynchronously from the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all entities of type <typeparamref name="T"/>.</returns>
        public Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its ID asynchronously from the database.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the entity to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entity of type <typeparamref name="T"/> if found, or <see langword="null"/> if not found.</returns>
        public Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeUpdated">The entity of type <typeparamref name="T"/> with updated values to persist.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UpdateAsync(T entityToBeUpdated);

        /// <summary>
        /// Deletes an entity asynchronously from the database by its ID.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteAsync(int id);
    }
}
