using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IDataTransferService<EventDTO> _eventProcessor;
        public EventsController(IDataTransferService<EventDTO> eventProcessor)
        {
            _eventProcessor = eventProcessor;
        }

        // GET: api/Events
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                List<EventDTO> events = await _eventProcessor.GetAllAsync();

                if (events.Count == 0)
                {
                    return NoContent();
                }

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        // GET api/Events/{id}
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
                EventDTO eventToGet = await _eventProcessor.GetByIdAsync(id);

                return Ok(eventToGet);
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                return NotFound(keyNotFoundException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        // POST api/Events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] EventDTO eventToBeAdded)
        {
            try
            {
                EventDTO addedEvent = await _eventProcessor.CreateAsync(eventToBeAdded);

                return Created("api/Events/" + addedEvent.EventId, addedEvent);
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(argumentNullException.Message);
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
            catch (DbUpdateException dbUpdateException)
            {
                return StatusCode(500, "Failed to save the event due to a database error: " + dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }

        }

        // PUT api/Events/{id}
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] EventDTO eventToBeUpdated)
        {
            if (id != eventToBeUpdated.EventId)
            {
                return BadRequest("Try again, ID in the URL does not match the Event ID.");
            }

            try
            {
                EventDTO updatedEvent = await _eventProcessor.UpdateAsync(id, eventToBeUpdated);

                return Ok(updatedEvent);
            }
            catch (ArgumentNullException argumentNullException)
            {
                return BadRequest(argumentNullException.Message);
            }
            catch (ArgumentException argumentException)
            {
                return BadRequest(argumentException.Message);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                return NotFound(keyNotFoundException.Message);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException) // Handle concurrency conflicts
            {
                return Conflict("Event was modified by another user. Refresh and retry: " + dbUpdateConcurrencyException.Message);
            }
            catch (DbUpdateException dbUpdateException) // Handle other database errors
            {
                return StatusCode(500, "Failed to update the event due to a database error: " + dbUpdateException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        // DELETE api/Events/{id}
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                EventDTO eventToBeDeleted = await _eventProcessor.DeleteAsync(id);
                return Ok(eventToBeDeleted);
            }
            catch (ArgumentException argumentException)
            {
                return NotFound(argumentException.Message);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                return NotFound(keyNotFoundException.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Failed to delete the event due to a database error.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred: " + ex.Message);
            }
        }

        //Filter http methods below (to be done...)
    }
}
