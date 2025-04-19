using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Events;
using Microsoft.AspNetCore.Mvc;

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling HTTP requests related to Events.
    /// Provides endpoints for CRUD operations on event resources.
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        /// <summary>
        /// Initializes a new instance of the EventsController class.
        /// </summary>
        /// <param name="eventService">The service for event operations.</param>
        /// <param name="logger">The logger for recording diagnostic information.</param>
        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        //TODO: Allow Anonymous
        /// <summary>
        /// Retrieves all events from the system.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of events if found,
        /// 204 No Content if no events exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() // YourApi.com/api/events
        {
            var result = await _eventService.GetAllAsync();

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                default:
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Anonymous
        /// <summary>
        /// Retrieves a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>
        /// 200 OK with the event data if found,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the event doesn't exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id) // YourApi.com/api/events/{id}
        {
            var result = await _eventService.GetByIdAsync(id);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting event {EventId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Creates a new event in the system.
        /// </summary>
        /// <param name="createRequest">The event data for creating a new event.</param>
        /// <returns>
        /// 201 Created with the created event data and location,
        /// 400 Bad Request if the request data is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateEventRequest createRequest) // YourApi.com/api/events
        {
            var result = await _eventService.CreateAsync(createRequest);

            switch (result.StatusCode)
            {
                case 201:
                    return Created("api/events/" + result.Data.EventId, result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error creating event: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Updates an existing event in the system.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="updateRequest">The updated event data.</param>
        /// <returns>
        /// 200 OK with the updated event data,
        /// 400 Bad Request if the request data is invalid or IDs don't match,
        /// 404 Not Found if the event doesn't exist,
        /// 409 Conflict if there's a concurrency issue,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateEventRequest updateRequest)// YourApi.com/api/events/{id}
        {
            if (id != updateRequest.EventId)
            {
                return BadRequest("ID mismatch between route parameter and event data.");
            }

            var result = await _eventService.UpdateAsync(updateRequest);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                case 409:
                    return Conflict(result.ErrorMessage);
                default:
                    _logger.LogError("Error updating event {EventId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Deletes an event from the system.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>
        /// 200 OK with the deleted event data,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the event doesn't exist,
        /// 409 Conflict if there's a concurrency issue or if the event cannot be deleted,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id) // YourApi.com/api/events/{id}
        {
            var result = await _eventService.DeleteAsync(new DeleteEventRequest(id));

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                case 409:
                    return Conflict(result.ErrorMessage);
                default:
                    _logger.LogError("Error deleting event {EventId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Updates the status of a specific event.
        /// </summary>
        /// <param name="id">The ID of the event whose status is to be updated.</param>
        /// <param name="request">The request containing the new status.</param>
        /// <returns>
        /// 200 OK with the updated event data if successful,
        /// 400 Bad Request if the request data is invalid or IDs don't match,
        /// 404 Not Found if the event doesn't exist,
        /// 409 Conflict if there's a concurrency issue or the status cannot be updated,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPatch("{id}/status")] // YourApi.com/api/events/{id}/status
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchStatusAsync(int id, [FromBody] UpdateEventStatusRequest request)
        {
            if (id != request.EventId)
            {
                return BadRequest("ID mismatch between route parameter and request data.");
            }

            var result = await _eventService.UpdateStatusAsync(request);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                case 409:
                    return Conflict(result.ErrorMessage);
                default:
                    _logger.LogError("Error updating status for event {EventId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}