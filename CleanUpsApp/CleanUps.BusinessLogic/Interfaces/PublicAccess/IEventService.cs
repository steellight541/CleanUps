using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    /// <summary>
    /// Defines a contract for managing event-related operations in the CleanUps application.
    /// This interface provides asynchronous methods for creating, reading, updating, and deleting events.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Creates a new event asynchronously in the CleanUps application.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> containing the event data to create.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EventDTO> CreateAsync(EventDTO eventDto);

        /// <summary>
        /// Retrieves all events asynchronously from the CleanUps application.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of <see cref="EventDTO"/> objects representing all events.</returns>
        public Task<List<EventDTO>> GetAllAsync();

        /// <summary>
        /// Retrieves an event by its ID asynchronously from the CleanUps application.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the event to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the <see cref="EventDTO"/> representing the event with the specified ID.</returns>
        public Task<EventDTO> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing event asynchronously in the CleanUps application.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> containing the updated event data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EventDTO> UpdateAsync(EventDTO eventDto);

        /// <summary>
        /// Deletes an event asynchronously from the CleanUps application by its ID.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the event to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EventDTO> DeleteAsync(int id);
    }
}
