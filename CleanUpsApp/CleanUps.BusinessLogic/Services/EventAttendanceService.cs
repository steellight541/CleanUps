using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class EventAttendanceService : IEventAttendanceService
    {
        private readonly IEventAttendanceRepository _repository;
        private readonly IEventAttendanceValidator _validator;
        private readonly IEventAttendanceConverter _converter;

        public EventAttendanceService(IEventAttendanceRepository repository, IEventAttendanceValidator validator, IEventAttendanceConverter converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        public async Task<Result<List<EventAttendanceDTO>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            return await _repository.GetAllAsync();
        }

        public async Task<Result<EventAttendanceDTO>> GetByIdAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Service: GetByIdAsync Method is not implemented, use another method.");
        }


        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            var idValidation = _validator.ValidateId(userId);
            if (!idValidation.IsSuccess)
            {
                return Result<List<Event>>.BadRequest(idValidation.ErrorMessage);
            }

            return await _repository.GetEventsByUserIdAsync(userId);
        }

        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
        {
            var idValidation = _validator.ValidateId(eventId);
            if (!idValidation.IsSuccess)
            {
                return Result<List<User>>.BadRequest(idValidation.ErrorMessage);
            }
            return await _repository.GetUsersByEventIdAsync(eventId);
        }


        public Result<int> GetAttendanceCountByEventId(int eventId)
        {
            var idValidation = _validator.ValidateId(eventId);
            if (!idValidation.IsSuccess)
            {
                return Result<int>.BadRequest(idValidation.ErrorMessage);
            }
            return _repository.GetAttendanceCountByEventId(eventId);
        }

        public async Task<Result<EventAttendanceDTO>> CreateAsync(EventAttendanceDTO dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceDTO>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            EventAttendance eventAttendanceModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(eventAttendanceModel);
        }

        public async Task<Result<EventAttendanceDTO>> UpdateAsync(EventAttendanceDTO entity)
        {
            return Result<EventAttendanceDTO>.InternalServerError("Service: UpdateAsync Method with one Id paramter is not implemented, use another method.");
        }

        public async Task<Result<EventAttendanceDTO>> UpdateAttendanceAsync(int userId, int eventId, EventAttendanceDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateEventAttendanceForUpdate(userId, eventId, dto);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceDTO>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            EventAttendance eventAttendanceModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(eventAttendanceModel);
        }
        public async Task<Result<EventAttendanceDTO>> DeleteAsync(EventAttendanceDTO attendance)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var eventIdValidation = _validator.ValidateId(attendance.EventId);
            if (!eventIdValidation.IsSuccess)
            {
                return Result<EventAttendanceDTO>.BadRequest(eventIdValidation.ErrorMessage);
            }

            var userIdValidation = _validator.ValidateId(attendance.UserId);
            if (!userIdValidation.IsSuccess)
            {
                return Result<EventAttendanceDTO>.BadRequest(userIdValidation.ErrorMessage);
            }

            var convertedModelResult = _converter.ToModel(attendance);

            await _repository.DeleteAsync(convertedModelResult);

            var convertedResponseResult = _converter.ToDTO(convertedModelResult);

            //Step 2. Pass the model to the repository - return result of operation
            return Result<EventAttendanceDTO>.Ok(convertedResponseResult);
        }
    }
}
