using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous] // This endpoint must be accessible without a token
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid) // Basic validation
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request);
            switch (result.StatusCode)
            {
                // Using StatusCodes constants is slightly cleaner than magic numbers
                case StatusCodes.Status200OK:
                    return Ok(result.Data); // Return LoginResponseDTO
                case StatusCodes.Status401Unauthorized:
                    return Unauthorized(result.ErrorMessage);
                case StatusCodes.Status400BadRequest:
                    return BadRequest(result.ErrorMessage);
                case StatusCodes.Status500InternalServerError:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage); // Handle other potential codes
            }
        }

        // Optional: Add a [HttpPost("register")] endpoint here later
        // [HttpPost("register")]
        // [AllowAnonymous]
        // public async Task<IActionResult> Register([FromBody] UserCreateDTO request) { ... }

        // Optional: Add an endpoint to test authentication
        [HttpGet("me")]
        [Authorize] // Requires a valid token
        public IActionResult GetCurrentUser()
        {
            // Access user claims injected by the authentication middleware
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
            var name = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (userId == null)
            {
                // Should not happen if [Authorize] is working correctly
                return Unauthorized("User information not found in token.");
            }

            return Ok(new { UserId = userId, Email = email, Name = name, Role = role });
        }
    }
}
