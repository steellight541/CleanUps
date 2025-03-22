using CleanUps.Shared.DTOs;

namespace CleanUps.BusinessLogic.Interfaces.PublicAccess
{
    /// <summary>
    /// Defines a contract for validating event data in the CleanUps application.
    /// This interface provides methods to validate <see cref="EventDTO"/> objects and event IDs before processing.
    /// </summary>
    public interface IEventValidator
    {
        /// <summary>
        /// Validates an <see cref="EventDTO"/> for creating a new event.
        /// Ensures the DTO meets the requirements for creating a new event, such as having an unset <see cref="EventDTO.EventId"/>.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> to validate for creation.</param>
        public void ValidateForCreate(EventDTO eventDto);

        /// <summary>
        /// Validates an <see cref="EventDTO"/> for updating an existing event.
        /// Ensures the DTO meets the requirements for updating an event, such as having a valid <see cref="EventDTO.EventId"/>.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> to validate for updating.</param>
        public void ValidateForUpdate(EventDTO eventDto);

        /// <summary>
        /// Validates an event ID to ensure it is a positive integer.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the event to validate.</param>
        public void ValidateId(int id);
    }
}
