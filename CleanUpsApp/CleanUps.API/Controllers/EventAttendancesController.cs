using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling HTTP requests related to Event Attendances.
    /// Provides endpoints for managing the relationship between users and events they attend.
    /// </summary>
    [Route("api/eventattendances")]
    [ApiController]
    public class EventAttendancesController : ControllerBase
    {
        private readonly IEventAttendanceService _eventAttendanceService;
        private readonly ILogger<EventAttendancesController> _logger;

        /// <summary>
        /// Initializes a new instance of the EventAttendancesController class.
        /// </summary>
        /// <param name="eventAttendanceService">The service for event attendance operations.</param>
        /// <param name="logger">The logger for recording diagnostic information.</param>
        public EventAttendancesController(IEventAttendanceService eventAttendanceService, ILogger<EventAttendancesController> logger)
        {
            _eventAttendanceService = eventAttendanceService;
            _logger = logger;
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Retrieves all event attendances from the system.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of event attendances if found,
        /// 204 No Content if no attendances exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() // YourApi.com/api/eventattendances
        {
            var result = await _eventAttendanceService.GetAllAsync();

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                default:
                    _logger.LogError("Error getting attendances: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer & Volunteer
        /// <summary>
        /// Retrieves all events that a specific user is attending.
        /// </summary>
        /// <param name="userId">The ID of the user whose events to retrieve.</param>
        /// <returns>
        /// 200 OK with a list of events if found,
        /// 204 No Content if the user is not attending any events,
        /// 400 Bad Request if the user ID is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [Route("user/{userId}/events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEventsForASingleUserAsync(int userId) // YourApi.com/api/eventattendances/user/{userId}/events
        {
            var result = await _eventAttendanceService.GetEventsByUserIdAsync(userId);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting events: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Retrieves all users attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose attendees to retrieve.</param>
        /// <returns>
        /// 200 OK with a list of users if found,
        /// 204 No Content if no users are attending the event,
        /// 400 Bad Request if the event ID is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [Route("event/{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersForASingleEventAsync(int eventId) // YourApi.com/api/eventattendances/event/{eventId}/users
        {
            var result = await _eventAttendanceService.GetUsersByEventIdAsync(eventId);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting users: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer & Volunteer
        /// <summary>
        /// Registers a user for an event (creates an event attendance record).
        /// </summary>
        /// <param name="createRequest">The data specifying which user is attending which event.</param>
        /// <returns>
        /// 201 Created with the created attendance data and location,
        /// 400 Bad Request if the request data is invalid,
        /// 404 Not Found if the user or event doesn't exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateEventAttendanceRequest createRequest) // YourApi.com/api/eventattendances
        {
            var result = await _eventAttendanceService.CreateAsync(createRequest);

            switch (result.StatusCode)
            {
                case 201:
                    return Created("api/eventattendances/" + result.Data.EventId, result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                default:
                    _logger.LogError("Error creating attendance: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Updates an event attendance record (typically to check in a user).
        /// </summary>
        /// <param name="userId">The ID of the user whose attendance to update.</param>
        /// <param name="eventId">The ID of the event for which to update attendance.</param>
        /// <param name="updateRequest">The updated event attendance data.</param>
        /// <returns>
        /// 200 OK with the updated attendance data,
        /// 400 Bad Request if the request data is invalid or IDs don't match,
        /// 404 Not Found if the attendance record doesn't exist,
        /// 409 Conflict if there's a concurrency issue,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPut]
        [Route("user/{userId}/event/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int userId, int eventId, [FromBody] UpdateEventAttendanceRequest updateRequest) // YourApi.com/api/eventattendances/user/{userId}/event/{eventId}
        {
            if (userId != updateRequest.UserId && eventId != updateRequest.EventId)
            {
                return BadRequest($"The parameter User-Id: {userId} and parameter Event-Id{eventId} does not match the data User-Id: {updateRequest.UserId} and data Event-Id: {updateRequest.EventId}");
            }
            var result = await _eventAttendanceService.UpdateAsync(updateRequest);

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
                    _logger.LogError("Error updating attendance for User-Id-{userId} at the event-id{eventId}: {StatusCode} - {Message}", userId, eventId, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer
        /// <summary>
        /// Deletes an event attendance record (cancels a user's attendance at an event).
        /// </summary>
        /// <param name="userId">The ID of the user whose attendance to delete.</param>
        /// <param name="eventId">The ID of the event from which to remove the user.</param>
        /// <returns>
        /// 200 OK with the deleted attendance data,
        /// 400 Bad Request if the IDs are invalid,
        /// 404 Not Found if the attendance record doesn't exist,
        /// 409 Conflict if there's a concurrency issue or if the record cannot be deleted,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpDelete]
        [Route("user/{userId}/event/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int userId, int eventId) // YourApi.com/api/eventattendances/user/{userId}/event/{eventId}
        {
            var result = await _eventAttendanceService.DeleteAsync(new DeleteEventAttendanceRequest(userId, eventId));

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
                    _logger.LogError("Error deleting attendance for User-Id-{userId} at the event-id{eventId}: {StatusCode} - {Message}", userId, eventId, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
