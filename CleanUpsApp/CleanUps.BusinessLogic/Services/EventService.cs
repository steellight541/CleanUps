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
            Result<List<Event>> repoResult = await _repository.GetAllAsync();
            return repoResult.Transform(events => _converter.ToResponseList(events));
        }

        /// <summary>
        /// Retrieves a single event by its ID and returns it as an <see cref="EventResponse"/> object.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>A <see cref="Result{T}"/> containing the <see cref="EventResponse"/> if found, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> GetByIdAsync(int id)
        {
            var validationResult = _validator.ValidateId(id);
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }
            Result<Event> repoResult = await _repository.GetByIdAsync(id);
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Creates a new event based on the provided <see cref="CreateEventRequest"/> and returns the created event.
        /// </summary>
        /// <param name="createRequest">The <see cref="CreateEventRequest"/> containing the data for the new event.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> CreateAsync(CreateEventRequest createRequest)
        {
            var validationResult = _validator.ValidateForCreate(createRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }
            Event eventModel = _converter.ToModel(createRequest);
            var repoResult = await _repository.CreateAsync(eventModel);
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Updates an existing event based on the provided <see cref="UpdateEventRequest"/> and returns the updated event.
        /// </summary>
        /// <param name="updateRequest">The <see cref="UpdateEventRequest"/> containing the updated event data.</param>
        /// <returns>A <see cref="Result{T}"/> containing the updated <see cref="EventResponse"/> if successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> UpdateAsync(UpdateEventRequest updateRequest)
        {
            var validationResult = _validator.ValidateForUpdate(updateRequest);
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }
            Event eventModel = _converter.ToModel(updateRequest);
            var repoResult = await _repository.UpdateAsync(eventModel);
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }

        /// <summary>
        /// Deletes an event based on the provided <see cref="DeleteEventRequest"/> and returns the result of the operation.
        /// </summary>
        /// <param name="deleteRequest">An object containing the ID of the event to delete.</param>
        /// <returns>A <see cref="Result{T}"/> containing the deleted event's data as <see cref="EventResponse"/> if the deletion is successful, or an error message if the operation fails.</returns>
        public async Task<Result<EventResponse>> DeleteAsync(DeleteEventRequest deleteRequest)
        {
            var validationResult = _validator.ValidateId(deleteRequest.EventId);
            if (!validationResult.IsSuccess)
            {
                return Result<EventResponse>.BadRequest(validationResult.ErrorMessage);
            }
            Result<Event> repoResult = await _repository.DeleteAsync(deleteRequest.EventId);
            return repoResult.Transform(eventObj => _converter.ToResponse(eventObj));
        }
    }
}