using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class EventService : IService<Event, EventDTO>
    {
        private readonly IRepository<Event> _repository;
        private readonly IValidator<EventDTO> _validator;
        private readonly IMapper<Event, EventDTO> _mapper;

        public EventService(IRepository<Event> repository, IValidator<EventDTO> validator, IMapper<Event, EventDTO> mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<Event>> CreateAsync(EventDTO dto)
        {
            //Step 1. Validate DTO from the parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Event>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Event eventModel = _mapper.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            var createResult = await _repository.CreateAsync(eventModel);
            if (!createResult.IsSuccess)
            {
                return Result<Event>.InternalServerError(createResult.ErrorMessage);
            }
            else
            {
                return Result<Event>.Created(createResult.Data);
            }
        }

        public async Task<Result<Event>> GetByIdAsync(int id)
        {
            //Step 1. Validate id from the parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Event>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            var repoResult = await _repository.GetByIdAsync(id);
            if (!repoResult.IsSuccess)
            {
                return repoResult.StatusCode == 404
                    ? Result<Event>.NotFound(repoResult.ErrorMessage)
                    : Result<Event>.InternalServerError(repoResult.ErrorMessage);
            }
            else
            {
                return Result<Event>.Ok(repoResult.Data);
            }
        }

        public async Task<Result<List<Event>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            var repoResult = await _repository.GetAllAsync();
            if (!repoResult.IsSuccess)
            {
                return Result<List<Event>>.InternalServerError(repoResult.ErrorMessage);
            }
            else
            {
                return Result<List<Event>>.Ok(repoResult.Data);
            }
        }

        public async Task<Result<Event>> UpdateAsync(int id, EventDTO dto)
        {
            //Step 1. Validate DTO from the parameter - return result of the validation
            var validationResult = _validator.ValidateForUpdate(id, dto);
            if (!validationResult.IsSuccess)
            {
                return Result<Event>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            Event eventModel = _mapper.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            var updateResult = await _repository.UpdateAsync(eventModel);
            if (!updateResult.IsSuccess)
            {
                return updateResult.StatusCode == 404
                    ? Result<Event>.NotFound(updateResult.ErrorMessage)
                    : Result<Event>.InternalServerError(updateResult.ErrorMessage);
            }
            else
            {
                return Result<Event>.Ok(updateResult.Data);
            }
        }

        public async Task<Result<Event>> DeleteAsync(int id)
        {
            //Step 1. Validate id from the parameter - return result of the validation
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return Result<Event>.BadRequest(idValidation.ErrorMessage);
            }

            //Step 2. Pass the id to the repository - return result of operation
            var deleteResult = await _repository.DeleteAsync(id);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult.StatusCode == 404
                    ? Result<Event>.NotFound(deleteResult.ErrorMessage)
                    : Result<Event>.InternalServerError(deleteResult.ErrorMessage);
            }
            else
            {
                return Result<Event>.Ok(deleteResult.Data);
            }
        }
    }
}