using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling HTTP requests related to authentication.
    /// Provides endpoints for login, session management, and password reset operations.
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

        /// <summary>
        /// Initiates a password reset request for the given email address.
        /// </summary>
        /// <param name="request">The request containing the user's email.</param>
        /// <returns>200 OK regardless of whether the email exists, to prevent email enumeration.</returns>
        [HttpPost("request-password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestPasswordReset([FromBody] EmailPasswordResetRequest request)
        {
            var result = await _authService.RequestPasswordResetAsync(request);

            if (!result.IsSuccess && result.StatusCode == 400)
            {
                return BadRequest(result.ErrorMessage);
            }

            if (!result.IsSuccess && result.StatusCode != 200)
            {
                _logger.LogError("Internal error during password reset request for {Email}: {StatusCode} - {Message}", request.Email, result.StatusCode, result.ErrorMessage);
            }

            return Ok("If an account with this email exists, a password reset token has been sent.");
        }

        /// <summary>
        /// Validates a password reset token.
        /// </summary>
        /// <param name="request">The request containing the token.</param>
        /// <returns>200 OK if the token is valid, otherwise an appropriate error status.</returns>
        [HttpPost("validate-reset-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateTokenRequest request)
        {
            var result = await _authService.ValidateResetTokenAsync(request);

            return result.IsSuccess
                ? Ok()
                : StatusCode(result.StatusCode, result.ErrorMessage);
        }

        /// <summary>
        /// Resets the user's password using a valid token and the new password.
        /// </summary>
        /// <param name="request">The request containing the token and new password details.</param>
        /// <returns>200 OK if successful, otherwise an appropriate error status.</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            return result.IsSuccess
                ? Ok("Password has been successfully reset.")
                : StatusCode(result.StatusCode, result.ErrorMessage);
        }
    }
}