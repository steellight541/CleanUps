using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    [Route("api/eventattendances")]
    [ApiController]
    public class EventAttendancesController : ControllerBase
    {
        private readonly IEventAttendanceService _eventAttendanceService;
        private readonly ILogger<EventsController> _logger;

        public EventAttendancesController(IEventAttendanceService eventAttendanceService, ILogger<EventsController> logger)
        {
            _eventAttendanceService = eventAttendanceService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Organizer")] // Only users with the 'Organizer' role
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() //GetALl
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

        [HttpGet]
        [Authorize(Roles = "Organizer,Volunteer")]
        [Route("user/{userId}/events")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEventsForASingleUserAsync(int userId)
        {
            Result<List<Event>> result = await _eventAttendanceService.GetEventsForASingleUserAsync(userId);

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
        [HttpGet]
        [Route("event/{eventId}/users")]
        [Authorize(Roles = "Organizer")] // Only users with the 'Organizer' role
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersForASingleEventAsync(int eventId)
        {
            Result<List<User>> result = await _eventAttendanceService.GetUsersForASingleEventAsync(eventId);
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

        [HttpGet]
        [Route("event/{eventId}/users/count")]
        [AllowAnonymous] // Explicitly allow anonymous access
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNumberOfUsersForASingleEvent(int eventId)
        {
            Result<int> result = _eventAttendanceService.GetNumberOfUsersForASingleEvent(eventId);

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

        [HttpPost]
        [Authorize(Roles = "Volunteer,Organizer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] EventAttendanceDTO dto)
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

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Organizer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int eventId, int userId, [FromBody] EventAttendanceDTO dto)
        {

            if (eventId != dto.EventId && userId != dto.UserId)
            {
                return BadRequest("ID mismatch between route parameter and data.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _eventAttendanceService.UpdateEventAttendanceAsync(eventId, userId, dto);

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
                    _logger.LogError("Error updating attendance Parameter-Id-{ParamEventId}, Parameter-Id-{ParamEventId}, EventAttendance-EventId-{EventId}, EventAttendance-UserId-{UserId}: {StatusCode} - {Message}", eventId, userId, result.Data.EventId, result.Data.UserId, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Organizer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _eventAttendanceService.DeleteAsync(id);

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
                    _logger.LogError("Error deleting attendance {id}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
