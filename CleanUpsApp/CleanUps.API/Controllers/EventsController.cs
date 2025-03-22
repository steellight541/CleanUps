using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    /// <summary>
    /// Provides RESTful API endpoints for managing events.
    /// </summary>
    [Route("api/Events")]  
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Events
        /// <summary>
        /// Retrieves a list of all events.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing either:
        /// <list type="bullet">
        ///   <item><description><see cref="OkObjectResult"/> (HTTP 200) with a list of <see cref="EventDTO"/> if successful.</description></item>
        ///   <item><description><see cref="NoContentResult"/> (HTTP 204) if no events are found.</description></item>
        ///   <item><description><see cref="ObjectResult"/> (HTTP 500) if an internal error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                List<EventDTO> events = await _eventService.GetAllAsync();

                if (events.Count == 0)
                {
                    return NoContent();
                }

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/Events/{id}
        /// <summary>
        /// Retrieves a single event by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// A unique identifier of type <see cref="int"/> representing the event to retrieve.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing either:
        /// <list type="bullet">
        ///   <item><description><see cref="OkObjectResult"/> (HTTP 200) with the requested <see cref="EventDTO"/>.</description></item>
        ///   <item><description><see cref="NotFoundObjectResult"/> (HTTP 404) if the event cannot be found.</description></item>
        ///   <item><description><see cref="BadRequestObjectResult"/> (HTTP 400) if the request is invalid.</description></item>
        ///   <item><description><see cref="ObjectResult"/> (HTTP 500) if an internal error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                EventDTO eventToGet = await _eventService.GetByIdAsync(id);

                return Ok(eventToGet);
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/Events
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventToBeAdded">
        /// The <see cref="EventDTO"/> containing details of the event to create.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing either:
        /// <list type="bullet">
        ///   <item><description><see cref="CreatedResult"/> (HTTP 201) with the newly created <see cref="EventDTO"/>.</description></item>
        ///   <item><description><see cref="BadRequestObjectResult"/> (HTTP 400) if the provided data is invalid.</description></item>
        ///   <item><description><see cref="ObjectResult"/> (HTTP 500) if an internal error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] EventDTO eventToBeAdded)
        {
            try
            {
                EventDTO addedEvent = await _eventService.CreateAsync(eventToBeAdded);

                return Created("api/Events/" + addedEvent.EventId, addedEvent);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        // PUT api/Events/{id}
        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="eventToBeUpdated">
        /// The <see cref="EventDTO"/> containing updated details of the event.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing either:
        /// <list type="bullet">
        ///   <item><description><see cref="OkObjectResult"/> (HTTP 200) with the updated <see cref="EventDTO"/>.</description></item>
        ///   <item><description><see cref="BadRequestObjectResult"/> (HTTP 400) if the provided data is invalid.</description></item>
        ///   <item><description><see cref="NotFoundObjectResult"/> (HTTP 404) if the event to update cannot be found.</description></item>
        ///   <item><description><see cref="ObjectResult"/> (HTTP 500) if an internal error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync([FromBody] EventDTO eventToBeUpdated)
        {
            try
            {
                EventDTO updatedEvent = await _eventService.UpdateAsync(eventToBeUpdated);

                return Ok(updatedEvent);
            }
            catch (ArgumentException aEx)
            {
                return BadRequest(aEx.Message);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/Events/{id}
        /// <summary>
        /// Deletes an existing event by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of type <see cref="int"/> representing the event to delete.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing either:
        /// <list type="bullet">
        ///   <item><description><see cref="OkObjectResult"/> (HTTP 200) with the deleted <see cref="EventDTO"/>.</description></item>
        ///   <item><description><see cref="NotFoundObjectResult"/> (HTTP 404) if the event cannot be found.</description></item>
        ///   <item><description><see cref="ObjectResult"/> (HTTP 500) if an internal error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                EventDTO eventToBeDeleted = await _eventService.DeleteAsync(id);
                return Ok(eventToBeDeleted);
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //Filter http methods to be done...
    }
}
