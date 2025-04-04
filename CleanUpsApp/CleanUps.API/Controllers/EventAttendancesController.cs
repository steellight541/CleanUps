using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    [Route("api/eventattendances")]
    [ApiController]
    public class EventAttendancesController : ControllerBase
    {
        private readonly IEventAttendanceService _eventAttendanceService;
        private readonly ILogger<EventAttendancesController> _logger;

        public EventAttendancesController(IEventAttendanceService eventAttendanceService, ILogger<EventAttendancesController> logger)
        {
            _eventAttendanceService = eventAttendanceService;
            _logger = logger;
        }

        //TODO: Allow Organizer
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() // YourApi.com/api/eventattendances
        {
            Result<List<EventAttendance>> result = await _eventAttendanceService.GetAllAsync();

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
        [HttpGet]
        [Route("user/{userId}/events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEventsForASingleUserAsync(int userId) // YourApi.com/api/eventattendances/user/{userId}/events
        {
            Result<List<Event>> result = await _eventAttendanceService.GetEventsByUserIdAsync(userId);

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
        [HttpGet]
        [Route("event/{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersForASingleEventAsync(int eventId) // YourApi.com/api/eventattendances/event/{eventId}/users
        {
            Result<List<User>> result = await _eventAttendanceService.GetUsersByEventIdAsync(eventId);
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

        //TODO: Allow Anonymous
        [HttpGet]
        [Route("event/{eventId}/users/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserCountForEvent(int eventId)// YourApi.com/api/eventattendances/event/{eventId}/users/count
        {
            Result<int> result = _eventAttendanceService.GetAttendanceCountByEventId(eventId); 

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting number of users: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow Organizer & Volunteer
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] EventAttendanceDTO dto) // YourApi.com/api/eventattendances
        {
            Result<EventAttendance> result = await _eventAttendanceService.CreateAsync(dto);

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
        [HttpPut]
        [Route("user/{userId}/event/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int userId, int eventId, [FromBody] EventAttendanceDTO dto) // YourApi.com/api/eventattendances/user/{userId}/event/{eventId}
        {
            var result = await _eventAttendanceService.UpdateAttendanceAsync(userId, eventId, dto);

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
        [HttpDelete]
        [Route("user/{userId}/event/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int userId, int eventId) // YourApi.com/api/eventattendances/user/{userId}/event/{eventId}
        {
            var result = await _eventAttendanceService.DeleteAttendanceAsync(userId, eventId);

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
