using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling HTTP requests related to Users.
    /// Provides endpoints for CRUD operations on user resources.
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Initializes a new instance of the UsersController class.
        /// </summary>
        /// <param name="userService">The service for user operations.</param>
        /// <param name="authService">The service for authentication operations.</param>
        /// <param name="logger">The logger for recording diagnostic information.</param>
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //TODO: Organizer only
        /// <summary>
        /// Retrieves all users from the system.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of users if found,
        /// 204 No Content if no users exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
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
        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>
        /// 200 OK with the user data if found,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the user doesn't exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
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
        /// <summary>
        /// Creates a new user in the system (registers a new user) and automatically logs them in.
        /// </summary>
        /// <param name="createRequest">The user data for creating a new user.</param>
        /// <returns>
        /// 201 Created with the created user data and location, as well as login information,
        /// 400 Bad Request if the request data is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
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
        /// <summary>
        /// Updates an existing user in the system.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateRequest">The updated user data.</param>
        /// <returns>
        /// 200 OK with the updated user data,
        /// 400 Bad Request if the request data is invalid or IDs don't match,
        /// 404 Not Found if the user doesn't exist,
        /// 409 Conflict if there's a concurrency issue or email conflict,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
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
        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>
        /// 200 OK with the deleted user data,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the user doesn't exist,
        /// 409 Conflict if there's a concurrency issue or if the user cannot be deleted,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
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
