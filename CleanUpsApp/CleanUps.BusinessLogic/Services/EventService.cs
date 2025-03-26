using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Manages event-related operations by processing <see cref="EventDTO"/> objects,
    /// including creation, retrieval, update, and deletion.
    /// </summary>
    internal class EventService : IDataTransferService<EventDTO>
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IValidator<EventDTO> _eventDTOValidator;
        private readonly IMapper<Event, EventDTO> _eventMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repo">
        /// The <see cref="IRepository{ModelClass}"/> implementation used for event database operations.
        /// </param>
        /// <param name="validator">
        /// The <see cref="IValidator{DTORecord}"/> used to validate event data.
        /// </param>
        /// <param name="mapper">
        /// The <see cref="IMapper{ModelClass, DTORecord}"/> used to map between <see cref="Event"/> and <see cref="EventDTO"/>.
        /// </param>
        public EventService(IRepository<Event> repo, IValidator<EventDTO> validator, IMapper<Event, EventDTO> mapper)
        {
            _eventRepo = repo;
            _eventDTOValidator = validator;
            _eventMapper = mapper;
        }

        /// <summary>
        /// Creates a new event in the system asynchronously.
        /// </summary>
        /// <param name="eventDto">
        /// The <see cref="EventDTO"/> containing event details to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the created <see cref="EventDTO"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided DTO fails validation.
        /// </exception>
        public async Task<EventDTO> CreateAsync(EventDTO eventDto)
        {
            // Step 1: Validate the DTO
            _eventDTOValidator.ValidateForCreate(eventDto);

            // Step 2: Transform DTO to Domain Model
            Event eventToCreate = _eventMapper.ConvertToModel(eventDto);

            // Step 3: Save using the repository
            await _eventRepo.CreateAsync(eventToCreate);
            return eventDto;
        }

        /// <summary>
        /// Retrieves all events asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of all <see cref="EventDTO"/> objects.
        /// </returns>
        public async Task<List<EventDTO>> GetAllAsync()
        {
            // Step 1: Retrieve all events
            List<Event> events = await _eventRepo.GetAllAsync();

            // Step 2: Transform to DTOs
            return _eventMapper.ConvertToDTOList(events);
        }

        /// <summary>
        /// Retrieves a single event by its identifier asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the matching <see cref="EventDTO"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided identifier is not valid.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no event with the specified identifier is found.
        /// </exception>
        public async Task<EventDTO> GetByIdAsync(int id)
        {
            // Step 1: Validate the ID
            _eventDTOValidator.ValidateId(id);

            // Step 2: Retrieve the event
            Event eventResult = await _eventRepo.GetByIdAsync(id);
            if (eventResult == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }

            // Step 3: Transform to DTO
            return _eventMapper.ConvertToDTO(eventResult);
        }

        /// <summary>
        /// Updates an existing event asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to update.
        /// </param>
        /// <param name="eventDto">
        /// The <see cref="EventDTO"/> containing updated event details.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the updated <see cref="EventDTO"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided DTO fails validation.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no event with the specified identifier exists.
        /// </exception>
        public async Task<EventDTO> UpdateAsync(int id, EventDTO eventDto)
        {
            // Step 1: Validate the DTO
            _eventDTOValidator.ValidateForUpdate(id, eventDto);

            // Step 2: Check if the event exists
            Event existingEvent = await _eventRepo.GetByIdAsync(eventDto.EventId);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventDto.EventId} not found.");
            }

            // Step 3: Transform DTO to Domain Model
            Event eventToUpdate = _eventMapper.ConvertToModel(eventDto);

            // Step 4: Update using the repository
            await _eventRepo.UpdateAsync(eventToUpdate);
            return eventDto;

        }

        /// <summary>
        /// Deletes an event asynchronously.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the deleted <see cref="EventDTO"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided identifier is not valid.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no event with the specified identifier exists.
        /// </exception>
        public async Task<EventDTO> DeleteAsync(int id)
        {
            // Step 1: Validate the ID
            _eventDTOValidator.ValidateId(id);

            // Step 2: Check if the event exists
            Event eventToDelete = await _eventRepo.GetByIdAsync(id);
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
