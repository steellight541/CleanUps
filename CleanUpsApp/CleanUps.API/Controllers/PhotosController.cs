using CleanUps.BusinessLogic.Services.Interfaces;
using CleanUps.Shared.DTOs.Photos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
    {
        /// <summary>
        /// Controller responsible for handling HTTP requests related to Photos.
        /// Provides endpoints for CRUD operations on photo resources.
        /// </summary>
        [Route("api/photos")]
        [ApiController]
        public class PhotosController : ControllerBase
        {
        private readonly IPhotoService _photoService;
        private readonly ILogger<PhotosController> _logger;

        /// <summary>
        /// Initializes a new instance of the PhotosController class.
        /// </summary>
        /// <param name="photoService">The service for photo operations.</param>
        /// <param name="logger">The logger for recording diagnostic information.</param>
        public PhotosController(IPhotoService photoService, ILogger<PhotosController> logger)
        {
            _photoService = photoService;
            _logger = logger;
        }

        //TODO: Allow Anonymous
        /// <summary>
        /// Retrieves all photos from the system.
        /// </summary>
        /// <returns>
        /// 200 OK with a list of photos if found,
        /// 204 No Content if no photos exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() // YourApi.com/api/photos
        {
            var result = await _photoService.GetAllAsync();

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

        //TODO: Allow Anonymous
        /// <summary>
        /// Retrieves a specific photo by its ID.
        /// </summary>
        /// <param name="id">The ID of the photo to retrieve.</param>
        /// <returns>
        /// 200 OK with the photo data if found,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the photo doesn't exist,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int id) // YourApi.com/api/photos/{id}
        {
            var result = await _photoService.GetByIdAsync(id);

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

        //TODO: Allow Anonymous
        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose photos to retrieve.</param>
        /// <returns>
        /// 200 OK with a list of photos if found,
        /// 204 No Content if no photos exist for the event,
        /// 400 Bad Request if the event ID is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpGet]
        [Route("events/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPhotosByEventIdAsync(int eventId)// YourApi.com/api/photos/events/{id}
        {
            var result = await _photoService.GetPhotosByEventIdAsync(eventId);

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

        //TODO: Allow Organizer
        /// <summary>
        /// Creates a new photo in the system.
        /// </summary>
        /// <param name="createRequest">The photo data for creating a new photo.</param>
        /// <returns>
        /// 201 Created with the created photo data and location,
        /// 400 Bad Request if the request data is invalid,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] CreatePhotoRequest createRequest) // YourApi.com/api/photos
        {
            var result = await _photoService.CreateAsync(createRequest);

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

        //TODO: Allow Organizer
        /// <summary>
        /// Updates an existing photo in the system (typically to update its caption).
        /// </summary>
        /// <param name="id">The ID of the photo to update.</param>
        /// <param name="updateRequest">The updated photo data.</param>
        /// <returns>
        /// 200 OK with the updated photo data,
        /// 400 Bad Request if the request data is invalid or IDs don't match,
        /// 404 Not Found if the photo doesn't exist,
        /// 409 Conflict if there's a concurrency issue,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdatePhotoRequest updateRequest) // YourApi.com/api/photos/{id}
        {
            if (id != updateRequest.PhotoId)
            {
                return BadRequest("ID mismatch between route parameter and event data.");
            }

            var result = await _photoService.UpdateAsync(updateRequest);

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

        //TODO: Allow Organizer
        /// <summary>
        /// Deletes a photo from the system.
        /// </summary>
        /// <param name="id">The ID of the photo to delete.</param>
        /// <returns>
        /// 200 OK with the deleted photo data,
        /// 400 Bad Request if the ID is invalid,
        /// 404 Not Found if the photo doesn't exist,
        /// 409 Conflict if there's a concurrency issue or if the photo cannot be deleted,
        /// 500 Internal Server Error if an error occurs during processing.
        /// </returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)// YourApi.com/api/users/{id}
        {
            var result = await _photoService.DeleteAsync(new DeletePhotoRequest(id));

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
