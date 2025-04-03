using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

    namespace CleanUps.API.Controllers
    {
        [Route("api/photos")]
        [ApiController]
        public class PhotosController : ControllerBase
        {
        private readonly IPhotoService _photoService;
        private readonly ILogger<EventsController> _logger;

        public PhotosController(IPhotoService photoService, ILogger<EventsController> logger)
        {
            _photoService = photoService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous] // Explicitly allow anonymous access
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() //GetALl
        {
            Result<List<Photo>> result = await _photoService.GetAllAsync();

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                default:
                    _logger.LogError("Error getting photos: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet()]
        [Route("{id}")]
        [AllowAnonymous] // Explicitly allow anonymous access
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id) //GetById
        {
            Result<Photo> result = await _photoService.GetByIdAsync(id);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                case 404:
                    return NotFound(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting photo {PhotoId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("events/{eventId}")]
        [AllowAnonymous] // Explicitly allow anonymous access
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPhotosByEventIdAsync(int eventId)
        {
            Result<List<Photo>> result = await _photoService.GetPhotosByEventIdAsync(eventId);

            switch (result.StatusCode)
            {
                case 200:
                    return Ok(result.Data);
                case 204:
                    return NoContent();
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error getting photos {EventId}: {StatusCode} - {Message}", eventId, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Organizer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] PhotoDTO dto)
        {
            Result<Photo> result = await _photoService.CreateAsync(dto);

            switch (result.StatusCode)
            {
                case 201:
                    return Created("api/photos/" + result.Data.PhotoId, result.Data);
                case 400:
                    return BadRequest(result.ErrorMessage);
                default:
                    _logger.LogError("Error creating photo: {StatusCode} - {Message}", result.StatusCode, result.ErrorMessage);
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
        public async Task<IActionResult> PutAsync(int id, [FromBody] PhotoDTO dto)
        {
            if (id != dto.PhotoId)
            {
                return BadRequest("ID mismatch between route parameter and event data.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _photoService.UpdateAsync(id, dto);

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
                    _logger.LogError("Error updating photo {PhotoId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
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
            var result = await _photoService.DeleteAsync(id);

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
                    _logger.LogError("Error deleting photo {PhotoId}: {StatusCode} - {Message}", id, result.StatusCode, result.ErrorMessage);
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
