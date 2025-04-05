using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service class for managing event attendances operations, including retrieval, creation, updating, and deletion of attendances.
    /// </summary>
    internal class EventAttendanceService : IEventAttendanceService
    {
        private readonly IEventAttendanceRepository _repository;
        private readonly IEventAttendanceValidator _validator;
        private readonly IEventAttendanceConverter _attendanceConverter;
        IEventConverter _eventConverter;
        IUserConverter _userConverter;

        public EventAttendanceService(IEventAttendanceRepository repository,
                                        IEventAttendanceValidator validator,
                                        IEventAttendanceConverter attendanceConverter,
                                        IEventConverter eventConverter,
                                        IUserConverter userConverter)
        {
            _repository = repository;
            _validator = validator;
            _attendanceConverter = attendanceConverter;
            _eventConverter = eventConverter;
            _userConverter = userConverter;
        }

        /// <summary>
        /// Retrieves all event attendances from the repository and returns them as a list of <see cref="UpdateEventAttendanceRequest"/> objects.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UpdateEventAttendanceRequest"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<EventAttendanceResponse>>> GetAllAsync()
        {
            Result<List<EventAttendance>> repoResult = await _repository.GetAllAsync();
            return repoResult.Transform(attendances => _attendanceConverter.ToResponseList(attendances));
        }

        /// <summary>
        /// Retrieves all events attended by a specific user and returns them as a list of <see cref="EventResponse"/> objects.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve events for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            var validationResult = _validator.ValidateId(userId);
            if (!validationResult.IsSuccess)
            {
                return Result<List<EventResponse>>.BadRequest(validationResult.ErrorMessage);
            }
            Result<List<Event>> repoResult = await _repository.GetEventsByUserIdAsync(userId);
            return repoResult.Transform(events => _eventConverter.ToResponseList(events));
        }

        /// <summary>
        /// Retrieves all users attending a specific event and returns them as a list of <see cref="UserResponse"/> objects.
        /// </summary>
        /// <param name="eventId">The ID of the event to retrieve users for.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UserResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
        {
            var validationResult = _validator.ValidateId(eventId);
            if (!validationResult.IsSuccess)
            {
                return Result<List<UserResponse>>.BadRequest(validationResult.ErrorMessage);
            }
            Result<List<User>> repoResult = await _repository.GetUsersByEventIdAsync(eventId);
            return repoResult.Transform(users => _userConverter.ToResponseList(users));
        }

        /// <summary>
        /// Creates a new event attendance based on the provided <see cref="UpdateEventAttendanceRequest"/> and returns the created attendance.
        /// </summary>
        /// <param name="createRequest">The <see cref="UpdateEventAttendanceRequest"/> containing the data for the new attendance.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="UpdateEventAttendanceRequest"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventAttendanceResponse>> CreateAsync(CreateEventAttendanceRequest createRequest)
        {
            var validationResult = _validator.ValidateForCreate(createRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(validationResult.ErrorMessage);
            }

            EventAttendance attendanceModel = _attendanceConverter.ToModel(createRequest);
            Result<EventAttendance> repoResult = await _repository.CreateAsync(attendanceModel);
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        /// <summary>
        /// Updates an existing event attendance based on the provided <see cref="UpdateEventAttendanceRequest"/> and returns the updated attendance.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateEventAttendanceRequest"/> containing the updated attendance data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="UpdateEventAttendanceRequest"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventAttendanceResponse>> UpdateAsync(UpdateEventAttendanceRequest updateRequest)
        {
            var validationResult = _validator.ValidateForUpdate(updateRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(validationResult.ErrorMessage);
            }
            EventAttendance attendanceModel = _attendanceConverter.ToModel(updateRequest);
            Result<EventAttendance> repoResult = await _repository.UpdateAsync(attendanceModel);
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        /// <summary>
        /// Deletes an event attendance based on the provided <see cref="UpdateEventAttendanceRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">The <see cref="UpdateEventAttendanceRequest"/> containing the user ID and event ID of the attendance to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted <see cref="UpdateEventAttendanceRequest"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventAttendanceResponse>> DeleteAsync(DeleteEventAttendanceRequest deleteRequest)
        {
            var eventIdValidation = _validator.ValidateId(deleteRequest.EventId);
            if (!eventIdValidation.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(eventIdValidation.ErrorMessage);
            }
            var userIdValidation = _validator.ValidateId(deleteRequest.UserId);
            if (!userIdValidation.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(userIdValidation.ErrorMessage);
            }

            Result<EventAttendance> repoResult = await _repository.DeleteAsync(deleteRequest);
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        public async Task<Result<EventAttendanceResponse>> GetByIdAsync(int id)
        {
            return Result<EventAttendanceResponse>.InternalServerError("Method: GetByIdAsync(id) is not supported for this service.");
        }
    }
}