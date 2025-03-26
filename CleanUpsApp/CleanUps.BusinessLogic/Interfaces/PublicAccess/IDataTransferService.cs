using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    /// <summary>
    /// Defines an interface for processing data transfer objects (DTOs) of type <typeparamref name="T"/> by providing CRUD operations.
    /// </summary>
    /// <typeparam name="T">
    /// The DTO type that must inherit from <see cref="RecordDTO"/>.
    /// </typeparam>
    public interface IDataTransferService<T> where T : RecordDTO
    {
        /// <summary>
        /// Creates a new DTO asynchronously.
        /// </summary>
        /// <param name="dtoToCreate">
        /// The DTO to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the created DTO.
        /// </returns>
        public Task<T> CreateAsync(T dtoToCreate);

        /// <summary>
        /// Retrieves all DTOs asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all DTOs.
        /// </returns>
        public Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves a DTO by its identifier asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the DTO to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the DTO if found.
        /// </returns>
        public Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing DTO asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the DTO to update.
        /// </param>
        /// <param name="dtoToUpdate">
        /// The DTO with updated values.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the updated DTO.
        /// </returns>
        public Task<T> UpdateAsync(int id, T dtoToUpdate);

        /// <summary>
        /// Deletes a DTO by its identifier asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the DTO to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the deleted DTO.
        /// </returns>
        public Task<T> DeleteAsync(int id);
    }
}
