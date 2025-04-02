using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

    namespace CleanUps.API.Controllers
    {
        [Route("api/photos")]
        [ApiController]
        public class PhotosController : ControllerBase
        {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet]
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet()]
        [Route("{id}")]
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpGet]
        [Route("events/{eventId}")]
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] PhotoDTO dto)
        {
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }

        [HttpDelete]
        [Route("{id}")]
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
                    return StatusCode(result.StatusCode, result.ErrorMessage);
            }
        }
    }
}
