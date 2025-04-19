using CleanUps.BusinessLogic.Repositories.Interfaces;
using Microsoft.AspNetCore.Http; // Added for StatusCodes
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// API controller for triggering scheduled database maintenance and automation tasks.
    /// Provides endpoints to manually run jobs like nightly cleanup or event status updates.
    /// </summary>
    /// <remarks>
    /// Consider securing these endpoints (e.g., using [Authorize(Roles = "Admin")]) 
    /// if they should not be publicly accessible.
    /// </remarks>
    [Route("api/schedule-automations")]
    [ApiController]
    public class ScheduleAutomationsController : ControllerBase
    {
        private readonly IScheduleAutomationRepository _repository;
        private readonly ILogger<ScheduleAutomationsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleAutomationsController"/> class.
        /// </summary>
        /// <param name="repository">The repository for executing scheduled tasks.</param>
        /// <param name="logger">The logger for recording controller actions and errors.</param>
        public ScheduleAutomationsController(
            IScheduleAutomationRepository repository,
            ILogger<ScheduleAutomationsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Triggers the nightly cleanup process which removes old, soft-deleted records.
        /// Calls the relevant stored procedures via the repository.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating success (200 OK) or failure (500 Internal Server Error).</returns>
        /// <response code="200">Indicates the cleanup procedures were triggered successfully.</response>
        /// <response code="500">Indicates an error occurred during the cleanup process. Check server logs.</response>
        [HttpPost("nightlycleanup")] // Changed HttpGet to HttpPost as it performs an action
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> RunNightlyCleanup()
        {
            // Step 1: Log the API call initiation.
            _logger.LogInformation("API endpoint triggered: RunNightlyCleanup");
            
            // Step 2: Call the repository method to execute the cleanup procedures.
            bool success = await _repository.RunNightlyCleanupAsync();

            // Step 3: Check the result from the repository.
            if (success)
            {
                // Step 4a: Log success and return 200 OK.
                _logger.LogInformation("Nightly cleanup executed successfully via API call.");
                return Ok("Nightly cleanup ran successfully.");
            }
            else
            {
                // Step 4b: Log failure and return 500 Internal Server Error.
                // Specific error details are logged in the repository.
                _logger.LogError("Nightly cleanup failed when triggered via API call.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Nightly cleanup failed. Check logs for details.");
            }
        }

        /// <summary>
        /// Triggers the process to update event statuses based on current date and time.
        /// Calls the relevant stored procedure via the repository.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating success (200 OK) or failure (500 Internal Server Error).</returns>
        /// <response code="200">Indicates the status update procedure was triggered successfully.</response>
        /// <response code="500">Indicates an error occurred during the status update. Check server logs.</response>
        [HttpPost("statusupdate")] // Changed HttpGet to HttpPost as it performs an action
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> RunStatusUpdate()
        {
            // Step 1: Log the API call initiation.
            _logger.LogInformation("API endpoint triggered: RunStatusUpdate");
            
            // Step 2: Call the repository method to execute the status update procedure.
            bool success = await _repository.RunStatusUpdateAsync();

            // Step 3: Check the result from the repository.
            if (success)
            {
                // Step 4a: Log success and return 200 OK.
                _logger.LogInformation("Status update executed successfully via API call.");
                return Ok("Status update ran successfully.");
            }
            else
            {
                // Step 4b: Log failure and return 500 Internal Server Error.
                // Specific error details are logged in the repository.
                _logger.LogError("Status update failed when triggered via API call.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Status update failed. Check logs for details.");
            }
        }
    }
}
