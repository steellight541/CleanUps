using CleanUps.BusinessDomain.Models.Flags;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
[assembly: InternalsVisibleTo("CleanUps.Configuration")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    /// <summary>
    /// Defines a generic repository interface for performing CRUD (Create, Read, Update, Delete) operations 
    /// on entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the entity, which must inherit from <see cref="EFModel"/>.
    /// </typeparam>
    internal interface IRepository<T> where T : EFModel //Saying the generic type T is a class that EntityFramework created with scaffolding
    {
        /// <summary>
        /// Creates a new entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeCreated">
        /// The entity of type <typeparamref name="T"/> to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task CreateAsync(T entityToBeCreated);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/> asynchronously from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all entities.
        /// </returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its identifier asynchronously from the database.
        /// </summary>
        /// <param name="id">
        /// The identifier of the entity to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the entity if found; otherwise, <see langword="null"/>.
        /// </returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="T"/> asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeUpdated">
        /// The entity with updated values.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task UpdateAsync(T entityToBeUpdated);

        /// <summary>
        /// Deletes an entity of type <typeparamref name="T"/> asynchronously from the database using its identifier.
        /// </summary>
        /// <param name="id">
        /// The identifier of the entity to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task DeleteAsync(int id);
    }
}
