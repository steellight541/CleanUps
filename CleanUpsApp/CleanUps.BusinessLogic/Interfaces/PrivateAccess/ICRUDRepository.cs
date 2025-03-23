using CleanUps.BusinessDomain.Models.Flags;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.DataAccess")]
[assembly: InternalsVisibleTo("CleanUps.Configuration")]

namespace CleanUps.BusinessLogic.Interfaces.PrivateAccess
{

    /// <summary>
    /// Defines a generic repository interface for performing CRUD (Create, Read, Update, Delete) operations 
    /// on entities of type <typeparamref name="ModelClass"/>.
    /// </summary>
    /// <typeparam name="ModelClass">
    /// The type of the entity, which must inherit from <see cref="ModelFlag"/>.
    /// </typeparam>
    internal interface ICRUDRepository<ModelClass> where ModelClass : ModelFlag
    {
        /// <summary>
        /// Creates a new entity asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeCreated">
        /// The entity of type <typeparamref name="ModelClass"/> to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task CreateAsync(ModelClass entityToBeCreated);

        /// <summary>
        /// Retrieves all entities of type <typeparamref name="ModelClass"/> asynchronously from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all entities.
        /// </returns>
        Task<List<ModelClass>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="ModelClass"/> by its identifier asynchronously from the database.
        /// </summary>
        /// <param name="id">
        /// The identifier of the entity to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the entity if found; otherwise, <see langword="null"/>.
        /// </returns>
        Task<ModelClass> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing entity of type <typeparamref name="ModelClass"/> asynchronously in the database.
        /// </summary>
        /// <param name="entityToBeUpdated">
        /// The entity with updated values.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task UpdateAsync(ModelClass entityToBeUpdated);

        /// <summary>
        /// Deletes an entity of type <typeparamref name="ModelClass"/> asynchronously from the database using its identifier.
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
