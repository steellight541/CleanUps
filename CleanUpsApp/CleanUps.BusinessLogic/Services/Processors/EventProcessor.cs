using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Services.Processors
{
    /// <summary>
    /// Manages event-related operations in the CleanUps application.
    /// This service handles CRUD operations for events, delegating validation to <see cref="IEntityValidator"/>,
    /// transformation to <see cref="IEventMapper"/>, and database saving to <see cref="ICRUDRepository{T}"/>.
    /// </summary>
    internal class EventProcessor : IDTOProcessor<EventDTO>
    {
        private readonly ICRUDRepository<Event> _eventRepo;
        private readonly IDTOValidator<EventDTO> _eventDTOValidator;
        private readonly IMapper<Event, EventDTO> _eventMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProcessor"/> class.
        /// </summary>
        /// <param name="repo">The <see cref="ICRUDRepository{T}"/> implementation for event database operations.</param>
        /// <param name="validator">The <see cref="IEntityValidator"/> used to validate event data.</param>
        /// <param name="mapper">The <see cref="IEventMapper"/> used to map between <see cref="EventDTO"/> and <see cref="Event"/>.</param>
        public EventProcessor(ICRUDRepository<Event> repo, IDTOValidator<EventDTO> validator, IMapper<Event, EventDTO> mapper)
        {
            _eventRepo = repo;
            _eventDTOValidator = validator;
            _eventMapper = mapper;
        }
        /// <summary>
        /// Creates a new event in the CleanUps application.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> containing the event data to create.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="eventDto"/> fails validation (e.g., missing required fields or invalid data).</exception>
        public async Task<EventDTO> CreateAsync(EventDTO eventDto)
        {
            // Step 1: Validate the DTO
            _eventDTOValidator.ValidateForCreate(eventDto);

            // Step 2: Transform DTO to Domain Model
            var eventToCreate = _eventMapper.ConvertToModel(eventDto);

            // Step 3: Save using the repository
            await _eventRepo.CreateAsync(eventToCreate);
            return eventDto;
        }

        /// <summary>
        /// Retrieves all events from the CleanUps application.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of <see cref="EventDTO"/> objects representing all events.</returns>
        public async Task<List<EventDTO>> GetAllAsync()
        {
            // Step 1: Retrieve all events
            var events = await _eventRepo.GetAllAsync();

            // Step 2: Transform to DTOs
            return _eventMapper.ConvertToDTOList(events);
        }

        /// <summary>
        /// Retrieves an event by its ID from the CleanUps application.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the event to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the <see cref="EventDTO"/> representing the event with the specified ID.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no event with the specified <paramref name="id"/> is found.</exception>
        public async Task<EventDTO> GetByIdAsync(int id)
        {
            // Step 1: Validate the ID
            _eventDTOValidator.ValidateId(id);

            // Step 2: Retrieve the event
            var eventResult = await _eventRepo.GetByIdAsync(id);
            if (eventResult == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            // Step 3: Transform to DTO
            return _eventMapper.ConvertToDTO(eventResult);
        }

        /// <summary>
        /// Updates an existing event in the CleanUps application.
        /// </summary>
        /// <param name="eventDto">The <see cref="EventDTO"/> containing the updated event data.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="eventDto"/> fails validation (e.g., missing required fields or invalid data).</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no event with the specified ID in <paramref name="eventDto"/> is found.</exception>
        public async Task<EventDTO> UpdateAsync(int id, EventDTO eventDto)
        {
            // Step 1: Validate the DTO
            _eventDTOValidator.ValidateForUpdate(id, eventDto);

            // Step 2: Check if the event exists
            var existingEvent = await _eventRepo.GetByIdAsync(eventDto.EventId);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventDto.EventId} not found.");
            }

            // Step 3: Transform DTO to Domain Model
            var eventToUpdate = _eventMapper.ConvertToModel(eventDto);

            // Step 4: Update using the repository
            await _eventRepo.UpdateAsync(eventToUpdate);
            return eventDto;

        }

        /// <summary>
        /// Deletes an event from the CleanUps application by its ID.
        /// </summary>
        /// <param name="id">The <see cref="int"/> identifier of the event to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no event with the specified <paramref name="id"/> is found.</exception>
        public async Task<EventDTO> DeleteAsync(int id)
        {
            // Step 1: Validate the ID
            _eventDTOValidator.ValidateId(id);

            // Step 2: Check if the event exists
            var eventToDelete = await _eventRepo.GetByIdAsync(id);
            if (eventToDelete == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            // Step 3: Delete using the repository
            await _eventRepo.DeleteAsync(id);

            return _eventMapper.ConvertToDTO(eventToDelete);
        }

    }
}
