using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //TODO: Organizer only
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() // YourApi.com/api/users
        {
            var result = await _userService.GetAllAsync();

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                default:
                    _logger.LogError("Error getting users: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Organizer only
        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id) // YourApi.com/api/users/{id}
        {
            var result = await _userService.GetByIdAsync(id);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting user {UserId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Allow anonymous
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest createRequest) // YourApi.com/api/users
        {
            var result = await _userService.CreateAsync(createRequest);

            switch (result.StatusCode)
            {
                case 201:
                    return Created("api/users/" + result.Data.UserId, result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error creating user: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Organizer only
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateUserRequest updateRequest) // YourApi.com/api/users/{id}
        {
            if (id != updateRequest.UserId)
            {
                return BadRequest("ID mismatch between route parameter and event data.");
            }

            var result = await _userService.UpdateAsync(updateRequest);

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
                    _logger.LogError("Error updating user {UserId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        //TODO: Organizer only
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id) // YourApi.com/api/users/{id}
        {

            var result = await _userService.DeleteAsync(new DeleteUserRequest(id));

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
                    _logger.LogError("Error deleting user {UserId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
