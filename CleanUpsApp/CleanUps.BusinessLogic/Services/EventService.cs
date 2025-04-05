using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.AbstractDTOs;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
namespace CleanUps.BusinessLogic.Services
{
    internal class EventService : IEventService<EventResponse, CreateEventRequest, UpdateEventRequest, DeleteEventRequest>
    {
        private readonly IRepository<Event> _repository;
        private readonly IValidator<CreateEventRequest, UpdateEventRequest> _validator;
        private readonly IConverter<Event, EventResponse, CreateEventRequest, UpdateEventRequest> _converter;

        public EventService(IRepository<Event> repository, IValidator<CreateEventRequest, UpdateEventRequest> validator, IConverter<Event, EventResponse, CreateEventRequest, UpdateEventRequest> converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        public async Task<Result<List<Event>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            return await _repository.GetAllAsync();
        }


        public async Task<Result<Event>> GetByIdAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Event>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Result<Event>> CreateAsync(EventDTO dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Event>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Event eventModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(eventModel);
        }

        public async Task<Result<Event>> UpdateAsync(EventDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateForUpdate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Event>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Event eventModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(eventModel);
        }

        public async Task<Result<Event>> DeleteAsync(int id)
        {
            //Step 1. Validate id from parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Event>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            return await _repository.DeleteAsync(id);
        }
    }
}