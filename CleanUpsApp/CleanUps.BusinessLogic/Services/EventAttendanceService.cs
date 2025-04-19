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
    /// Service class providing business logic for managing event attendances.
    /// Handles operations like registering users for events, fetching attendees, and managing attendance records.
    /// </summary>
    internal class EventAttendanceService : IEventAttendanceService
    {
        private readonly IEventAttendanceRepository _repository;
        private readonly IEventAttendanceValidator _validator;
        private readonly IEventAttendanceConverter _attendanceConverter;
        private readonly IEventConverter _eventConverter;
        private readonly IUserConverter _userConverter;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAttendanceService"/> class.
        /// </summary>
        /// <param name="repository">Repository for data access to event attendance records.</param>
        /// <param name="validator">Validator for event attendance related DTOs.</param>
        /// <param name="attendanceConverter">Converter for EventAttendance models and DTOs.</param>
        /// <param name="eventConverter">Converter for Event models and DTOs (used for related data).</param>
        /// <param name="userConverter">Converter for User models and DTOs (used for related data).</param>
        /// <param name="emailService">Service for sending confirmation emails.</param>
        public EventAttendanceService(IEventAttendanceRepository repository,
                                        IEventAttendanceValidator validator,
                                        IEventAttendanceConverter attendanceConverter,
                                        IEventConverter eventConverter,
                                        IUserConverter userConverter,
                                        IEmailService emailService)
        {
            _repository = repository;
            _validator = validator;
            _attendanceConverter = attendanceConverter;
            _eventConverter = eventConverter;
            _userConverter = userConverter;
            _emailService = emailService;
        }

        /// <summary>
        /// Retrieves all event attendance records from the repository.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="EventAttendanceResponse"/> DTOs, or an error.</returns>
        public async Task<Result<List<EventAttendanceResponse>>> GetAllAsync()
        {
            // Step 1: Call repository to get all attendances.
            Result<List<EventAttendance>> repoResult = await _repository.GetAllAsync();

            // Step 2: Convert the result to response DTOs.
            // Step 3: Return the transformed result.
            return repoResult.Transform(attendances => _attendanceConverter.ToResponseList(attendances));
        }

        /// <summary>
        /// Retrieves all events attended by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="EventResponse"/> DTOs for the events the user attended, or an error.</returns>
        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            // Step 1: Validate the user ID.
            var validationResult = _validator.ValidateId(userId);
            
            // Step 2: If validation fails, return BadRequest.
            if (!validationResult.IsSuccess)
            {
                return Result<List<EventResponse>>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to get events by user ID.
            Result<List<Event>> repoResult = await _repository.GetEventsByUserIdAsync(userId);

            // Step 4: Convert the result to event response DTOs.
            // Step 5: Return the transformed result.
            return repoResult.Transform(events => _eventConverter.ToResponseList(events));
        }

        /// <summary>
        /// Retrieves all users attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="UserResponse"/> DTOs for the users attending the event, or an error.</returns>
        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
        {
            // Step 1: Validate the event ID.
            var validationResult = _validator.ValidateId(eventId);

            // Step 2: If validation fails, return BadRequest.
            if (!validationResult.IsSuccess)
            {
                return Result<List<UserResponse>>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to get users by event ID.
            Result<List<User>> repoResult = await _repository.GetUsersByEventIdAsync(eventId);

            // Step 4: Convert the result to user response DTOs.
            // Step 5: Return the transformed result.
            return repoResult.Transform(users => _userConverter.ToResponseList(users));
        }

        /// <summary>
        /// Creates a new event attendance record, effectively registering a user for an event.
        /// Sets the check-in time to the current UTC time upon creation.
        /// Sends a confirmation email to the user upon successful registration.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreateEventAttendanceRequest"/> containing user and event IDs.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="EventAttendanceResponse"/> DTO, or an error.</returns>
        /// <remarks>Email sending errors are logged but do not cause the overall operation to fail.</remarks>
        public async Task<Result<EventAttendanceResponse>> CreateAsync(CreateEventAttendanceRequest createRequest)
        {
            // Step 1: Validate the create request DTO.
            var validationResult = _validator.ValidateForCreate(createRequest);

            // Step 2: If validation fails, return BadRequest.
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to the domain model.
            EventAttendance attendanceModel = _attendanceConverter.ToModel(createRequest);

            // Step 4: Set the check-in time to the current UTC time.
            attendanceModel.CheckIn = DateTime.UtcNow;

            // Step 5: Call repository to create the attendance record.
            Result<EventAttendance> repoResult = await _repository.CreateAsync(attendanceModel);

            // Step 6: Check if creation was successful and data is available for email sending.
            if(repoResult.IsSuccess && repoResult.Data?.User != null && repoResult.Data?.Event != null)
            {
                 try
                {
                    // Step 7: Attempt to send a confirmation email.
                    await _emailService.SendEventAttendanceConfirmationEmailAsync(
                        repoResult.Data.User.Email,
                        repoResult.Data.User.Name,
                        repoResult.Data.Event.Title); // Assuming Event has a Title property
                }
                catch (Exception ex)
                {
                    // Log email failure but don't fail the overall operation
                    Console.WriteLine($"Error sending event attendance confirmation email: {ex.Message}");
                }
            }

            // Step 8: Convert the created/retrieved attendance record to a response DTO.
            // Step 9: Return the transformed result.
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        /// <summary>
        /// Updates an existing event attendance record.
        /// Typically used for modifying check-in status or other attendance details.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateEventAttendanceRequest"/> containing the updated data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="EventAttendanceResponse"/> DTO, or an error.</returns>
        public async Task<Result<EventAttendanceResponse>> UpdateAsync(UpdateEventAttendanceRequest updateRequest)
        {
            // Step 1: Validate the update request DTO.
            
            var validationResult = _validator.ValidateForUpdate(updateRequest); // Ensure validator exists/is correct
            // Step 2: If validation fails, return BadRequest.
            if (!validationResult.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to the domain model.
            EventAttendance attendanceModel = _attendanceConverter.ToModel(updateRequest);

            // Step 4: Call repository to update the attendance record.
            Result<EventAttendance> repoResult = await _repository.UpdateAsync(attendanceModel);

            // Step 5: Convert the updated attendance record to a response DTO.
            // Step 6: Return the transformed result.
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        /// <summary>
        /// Deletes an event attendance record, effectively unregistering a user from an event.
        /// Sends a confirmation email upon successful deletion.
        /// </summary>
        /// <param name="deleteRequest">The <see cref="DeleteEventAttendanceRequest"/> containing the user and event IDs.</param>
        /// <returns>A <see cref="Result{T}"/> containing the data of the deleted <see cref="EventAttendanceResponse"/> DTO, or an error.</returns>
        /// <remarks>Email sending errors are logged but do not cause the overall operation to fail.</remarks>
        public async Task<Result<EventAttendanceResponse>> DeleteAsync(DeleteEventAttendanceRequest deleteRequest)
        {
            // Step 1: Validate the event ID.
            var eventIdValidation = _validator.ValidateId(deleteRequest.EventId);

            // Step 2: If event ID validation fails, return BadRequest.
            if (!eventIdValidation.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(eventIdValidation.ErrorMessage);
            }

            // Step 3: Validate the user ID.
            var userIdValidation = _validator.ValidateId(deleteRequest.UserId);

            // Step 4: If user ID validation fails, return BadRequest.
            if (!userIdValidation.IsSuccess)
            {
                return Result<EventAttendanceResponse>.BadRequest(userIdValidation.ErrorMessage);
            }

            // Step 5: Call repository to delete the attendance record.
            Result<EventAttendance> repoResult = await _repository.DeleteAsync(deleteRequest);

            // Step 6: Check if deletion was successful and data is available for email sending.
            if(repoResult.IsSuccess && repoResult.Data?.User != null && repoResult.Data?.Event != null)
            {
                try
                {
                    // Step 7: Attempt to send a confirmation email.
                    await _emailService.SendEventAttendanceConfirmationEmailAsync(
                        repoResult.Data.User.Email, 
                        repoResult.Data.User.Name, 
                        repoResult.Data.Event.Title); // Assuming Event has a Title property
                }
                catch (Exception ex)
                {
                    // Step 8: Log email failure but don't fail the overall operation.
                    Console.WriteLine($"Error sending event attendance confirmation email: {ex.Message}");
                }
            }

            // Step 9: Convert the deleted attendance record to a response DTO.
            // Step 10: Return the transformed result.
            return repoResult.Transform(attendance => _attendanceConverter.ToResponse(attendance));
        }

        /// <summary>
        /// Retrieves a single event attendance by ID. NOTE: This method is not typically used as attendances are identified by UserID+EventID.
        /// </summary>
        /// <param name="id">The composite ID (not typically used).</param>
        /// <returns>A <see cref="Result{T}"/> indicating the method is not supported.</returns>
        public async Task<Result<EventAttendanceResponse>> GetByIdAsync(int id)
        {
            // Since attendances are identified by UserId+EventId, this method is not supported.
            // Return an error indicating this method is not supported.
            // Task.FromResult is used since the method is async but doesn't perform actual async operations.
            return await Task.FromResult(Result<EventAttendanceResponse>.InternalServerError("GetByIdAsync(id) is not supported for EventAttendanceService. Use methods involving UserId and EventId."));
        }
    }
}