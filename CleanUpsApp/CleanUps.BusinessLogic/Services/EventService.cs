using CleanUps.BusinessLogic.Converters.Interfaces;
using CleanUps.BusinessLogic.Models;
using CleanUps.BusinessLogic.Repositories.Interfaces;
using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.BusinessLogic.Validators.Interfaces;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanUps.Configuration")]
namespace CleanUps.BusinessLogic.Services
{
    /// <summary>
    /// Service class for managing event operations, including retrieval, creation, updating, and deletion of events.
    /// </summary>
    internal class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly IEventValidator _validator;
        private readonly IEventConverter _converter;

        public EventService(IEventRepository repository,
                            IEventValidator validator,
                            IEventConverter converter)
        {
            _repository = repository;
            _validator = validator;
            _converter = converter;
        }

        /// <summary>
        /// Retrieves all events from the repository and returns them as a list of <see cref="EventResponse"/> objects.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> containing a list of <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<List<EventResponse>>> GetAllAsync()
        {
            // Step 1: Call repository to retrieve all events.
            Result<List<Event>> repoResult = await _repository.GetAllAsync();
            
            // Step 2: Convert domain model events to response DTOs and return.
            return repoResult.Transform(events => _converter.ToResponseList(events));
        }

        /// <summary>
        /// Retrieves a single event by its ID and returns it as an <see cref="EventResponse"/> object.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="EventResponse"/> if found, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> GetByIdAsync(int id)
        {
            // Step 1: Validate the event ID.
            var validationResult = _validator.ValidateId(id);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to retrieve the event by ID.
            Result<Event> repoResult = await _repository.GetByIdAsync(id);

            // Step 4: Convert domain model event to response DTO and return.
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Creates a new event based on the provided <see cref="CreateEventRequest"/> and returns the created event.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreateEventRequest"/> containing the data for the new event.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> CreateAsync(CreateEventRequest createRequest)
        {
            // Step 1: Validate the create request DTO.
            var validationResult = _validator.ValidateForCreate(createRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to a domain model.
            Event eventModel = _converter.ToModel(createRequest);

            // Step 4: Call repository to create the event.
            var repoResult = await _repository.CreateAsync(eventModel);

            // Step 5: Convert domain model to response DTO and return.
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Updates an existing event based on the provided <see cref="UpdateEventRequest"/> and returns the updated event.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateEventRequest"/> containing the updated event data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> UpdateAsync(UpdateEventRequest updateRequest)
        {
            // Step 1: Validate the update request DTO.
            var validationResult = _validator.ValidateForUpdate(updateRequest);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Convert the request DTO to a domain model.
            Event eventModel = _converter.ToModel(updateRequest);

            // Step 4: Call repository to update the event.
            var repoResult = await _repository.UpdateAsync(eventModel);

            // Step 5: Convert domain model to response DTO and return.
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Deletes an event based on the provided <see cref="DeleteEventRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">An object containing the ID of the event to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted event's data as <see cref="EventResponse"/> if the deletion is successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> DeleteAsync(DeleteEventRequest deleteRequest)
        {
            // Step 1: Validate the event ID to delete.
            var validationResult = _validator.ValidateId(deleteRequest.EventId);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call repository to delete the event.
            Result<Event> repoResult = await _repository.DeleteAsync(deleteRequest.EventId);
            
            // Step 4: Convert domain model to response DTO and return.
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Updates the status of a specific event and returns the updated event.
        /// </summary>
        /// <param name="request">The request containing the event ID and the new status.</param>
        /// <returns>A Result containing the updated <see cref="EventResponse"/> if successful, or an error message otherwise.</returns>
        public async Task<Result<EventResponse>> UpdateStatusAsync(UpdateEventStatusRequest request)
        {
            // Step 1: Validate the update status request.
            var validationResult = _validator.ValidateForStatusUpdate(request);

            // Step 2: If validation fails, return BadRequest with error message.
            if (!validationResult.IsSuccess)
            {
                // Need to cast the error result to the correct type.
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }

            // Step 3: Call the repository to update the status.
            Result<Event> repoResult = await _repository.UpdateStatusAsync(request.EventId, (int)request.NewStatus);

            // Step 4: Convert the repository result (Event) to a service result (EventResponse).
            return repoResult.Transform(eventModel => _converter.ToResponse(eventModel));
        }
    }
}