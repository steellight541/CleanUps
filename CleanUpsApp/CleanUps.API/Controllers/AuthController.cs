using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling HTTP requests related to authentication.
    /// Provides endpoints for login and session management.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the AuthController class.
        /// </summary>
        /// <param name="authService">The service for authentication operations.</param>
        /// <param name="logger">The logger for recording diagnostic information.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login credentials (email and password).</param>
        /// <returns>
        /// 200 OK with the user session data if authentication is successful,
        /// 400 Bad Request if the request data is invalid,
        /// 401 Unauthorized if the credentials are invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 401:
                    return Unauthorized(result.ErrorMessage);
                default:
                    _logger.LogError("Error during login: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}