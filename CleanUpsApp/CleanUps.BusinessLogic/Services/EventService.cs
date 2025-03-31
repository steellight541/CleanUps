using CleanUps.BusinessDomain.Models;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    public class EventService : IDataTransferService<EventDTO>
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

        public async Task<OperationResult<EventDTO>> CreateAsync(EventDTO dto)
        {
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return OperationResult<EventDTO>.BadRequest(validationResult.ErrorMessage);
            }

            var entity = _mapper.ConvertToModel(dto);
            var createResult = await _repository.CreateAsync(entity);
            if (!createResult.IsSuccess)
            {
                return OperationResult<EventDTO>.InternalServerError(createResult.ErrorMessage);
            }

            var createdDto = _mapper.ConvertToDTO(createResult.Data);
            return OperationResult<EventDTO>.Created(createdDto);
        }

        public async Task<OperationResult<EventDTO>> GetByIdAsync(int id)
        {
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return OperationResult<EventDTO>.BadRequest(idValidation.ErrorMessage);
            }

            var repoResult = await _repository.GetByIdAsync(id);
            if (!repoResult.IsSuccess)
            {
                return repoResult.StatusCode == 404
                    ? OperationResult<EventDTO>.NotFound(repoResult.ErrorMessage)
                    : OperationResult<EventDTO>.InternalServerError(repoResult.ErrorMessage);
            }

            var dto = _mapper.ConvertToDTO(repoResult.Data);
            return OperationResult<EventDTO>.Ok(dto);
        }

        public async Task<OperationResult<List<EventDTO>>> GetAllAsync()
        {
            var repoResult = await _repository.GetAllAsync();
            if (!repoResult.IsSuccess)
            {
                return OperationResult<List<EventDTO>>.InternalServerError(repoResult.ErrorMessage);
            }

            var dtos = _mapper.ConvertToDTOList(repoResult.Data);
            return OperationResult<List<EventDTO>>.Ok(dtos);
        }

        public async Task<OperationResult<EventDTO>> UpdateAsync(int id, EventDTO dto)
        {
            var validationResult = _validator.ValidateForUpdate(id, dto);
            if (!validationResult.IsSuccess)
            {
                return OperationResult<EventDTO>.BadRequest(validationResult.ErrorMessage);
            }

            var entity = _mapper.ConvertToModel(dto);
            var updateResult = await _repository.UpdateAsync(entity);
            if (!updateResult.IsSuccess)
            {
                return updateResult.StatusCode == 404
                    ? OperationResult<EventDTO>.NotFound(updateResult.ErrorMessage)
                    : OperationResult<EventDTO>.InternalServerError(updateResult.ErrorMessage);
            }

            var updatedDto = _mapper.ConvertToDTO(updateResult.Data);
            return OperationResult<EventDTO>.Ok(updatedDto);
        }

        public async Task<OperationResult<EventDTO>> DeleteAsync(int id)
        {
            var idValidation = _validator.ValidateId(id);
            if (!idValidation.IsSuccess)
            {
                return OperationResult<EventDTO>.BadRequest(idValidation.ErrorMessage);
            }

            var deleteResult = await _repository.DeleteAsync(id);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult.StatusCode == 404
                    ? OperationResult<EventDTO>.NotFound(deleteResult.ErrorMessage)
                    : OperationResult<EventDTO>.InternalServerError(deleteResult.ErrorMessage);
            }

            var deletedDto = _mapper.ConvertToDTO(deleteResult.Data);
            return OperationResult<EventDTO>.Ok(deletedDto);
        }
    }
}