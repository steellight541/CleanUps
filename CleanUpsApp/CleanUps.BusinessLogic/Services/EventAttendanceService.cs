using CleanUps.BusinessLogic.Interfaces.PrivateAccess;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess.EventAttendanceInterfaces;
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;

namespace CleanUps.BusinessLogic.Services
{
    internal class EventAttendanceService : IEventAttendanceService
    {
        private readonly IEventAttendanceRepository _repository;
        private readonly IEventAttendanceValidator _validator;
        private readonly IConverter<EventAttendance, EventAttendanceDTO> _converter;

        public EventAttendanceService(IEventAttendanceRepository repository, IEventAttendanceValidator validator, IConverter<EventAttendance, EventAttendanceDTO> converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        public async Task<Result<List<EventAttendance>>> GetAllAsync()
        {
            //Step 1. Call GetAll from the repository - return result of operation
            return await _repository.GetAllAsync();
        }

        public async Task<Result<EventAttendance>> GetByIdAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Service: GetByIdAsync Method is not implemented, use another method.");
        }


        public async Task<Result<List<Event>>> GetEventsByUserIdAsync(int userId)
        {
            var idValidation = _validator.ValidateId(userId);
            if (!idValidation.IsSuccess)
            {
                return Result<List<Event>>.BadRequest(idValidation.ErrorMessage);
            }

            return await _repository.GetEventsByUserIdAsync(userId);
        }

        public async Task<Result<List<User>>> GetUsersByEventIdAsync(int eventId)
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

        public async Task<Result<EventAttendance>> CreateAsync(EventAttendanceDTO dto)
        {
            //Step 1. Validate DTO from parameter - return result of the validation
            var validationResult = _validator.ValidateForCreate(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendance>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            EventAttendance eventAttendanceModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.CreateAsync(eventAttendanceModel);
        }

        public async Task<Result<EventAttendance>> UpdateAsync(EventAttendanceDTO entity)
        {
            return Result<EventAttendance>.InternalServerError("Service: UpdateAsync Method with one Id paramter is not implemented, use another method.");
        }

        public async Task<Result<EventAttendance>> UpdateAttendanceAsync(int userId, int eventId, EventAttendanceDTO dto)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var validationResult = _validator.ValidateEventAttendanceForUpdate(userId, eventId, dto);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendance>.BadRequest(validationResult.ErrorMessage);
            }

            //Step 2. Convert DTO to Domain Model
            EventAttendance eventAttendanceModel = _converter.ConvertToModel(dto);

            //Step 3. Pass the model to the repository - return result of operation
            return await _repository.UpdateAsync(eventAttendanceModel);
        }
        public async Task<Result<EventAttendance>> DeleteAttendanceAsync(int userId, int eventId)
        {
            //Step 1. Validate DTO the parameter - return result of the validation
            var eventIdValidation = _validator.ValidateId(eventId);
            if (!eventIdValidation.IsSuccess)
            {
                return Result<EventAttendance>.BadRequest(eventIdValidation.ErrorMessage);
            }

            var userIdValidation = _validator.ValidateId(userId);
            if (!userIdValidation.IsSuccess)
            {
                return Result<EventAttendance>.BadRequest(userIdValidation.ErrorMessage);
            }

            //Step 2. Pass the model to the repository - return result of operation
            return await _repository.DeleteAttendanceAsync(userId, eventId);
        }

        public async Task<Result<EventAttendance>> DeleteAsync(int id)
        {
            return Result<EventAttendance>.InternalServerError("Service: DeleteAsync Method with one Id paramter is not implemented, use another method.");
        }
    }
}
