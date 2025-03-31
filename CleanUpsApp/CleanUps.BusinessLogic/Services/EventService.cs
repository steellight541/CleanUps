using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Test")]

namespace CleanUps.BusinessLogic.Services
{
    internal class EventService : IDataTransferService<EventDTO>
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IValidator<EventDTO> _eventDTOValidator;
        private readonly IMapper<Event, EventDTO> _eventMapper;

        public EventService(IRepository<Event> repo, IValidator<EventDTO> validator, IMapper<Event, EventDTO> mapper)
        {
            _eventRepo = repo;
            _eventDTOValidator = validator;
            _eventMapper = mapper;
        }

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

        public async Task<List<EventDTO>> GetAllAsync()
        {
            // Step 1: Retrieve all events
            List<Event> events = await _eventRepo.GetAllAsync();

            // Step 2: Transform to DTOs
            return _eventMapper.ConvertToDTOList(events);
        }

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
