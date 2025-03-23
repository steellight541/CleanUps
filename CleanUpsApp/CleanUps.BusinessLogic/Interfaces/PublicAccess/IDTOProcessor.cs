using CleanUps.Shared.DTOs.Flags;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    /// <summary>
    /// Defines an interface for processing data transfer objects (DTOs) of type <typeparamref name="DTORecord"/> by providing CRUD operations.
    /// </summary>
    /// <typeparam name="DTORecord">
    /// The DTO type that must inherit from <see cref="RecordFlag"/>.
    /// </typeparam>
    public interface IDTOProcessor<DTORecord> where DTORecord : RecordFlag
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
        public Task<DTORecord> CreateAsync(DTORecord dtoToCreate);

        /// <summary>
        /// Retrieves all DTOs asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all DTOs.
        /// </returns>
        public Task<List<DTORecord>> GetAllAsync();

        /// <summary>
        /// Retrieves a DTO by its identifier asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the DTO to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the DTO if found.
        /// </returns>
        public Task<DTORecord> GetByIdAsync(int id);

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
        public Task<DTORecord> UpdateAsync(int id, DTORecord dtoToUpdate);

        /// <summary>
        /// Deletes a DTO by its identifier asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the DTO to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the deleted DTO.
        /// </returns>
        public Task<DTORecord> DeleteAsync(int id);
    }
}
