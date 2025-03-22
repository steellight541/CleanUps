using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanUps.API.Controllers
{
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync()
        {
            List<EventDTO> events = await _eventService.GetAllAsync();

            return Ok(events);
        }

        // GET api/Events/{id}
        [HttpGet()]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(int id)
        {
            EventDTO eventToGet = await _eventService.GetByIdAsync(id);

            return Ok(eventToGet);
        }

        // POST api/Events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] EventDTO eventToBeAdded)
        {
            EventDTO addedEvent = await _eventService.CreateAsync(eventToBeAdded);

            return Created("api/Events/" + addedEvent.EventId, addedEvent);
        }

        // PUT api/Events/{id}
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromBody] EventDTO eventToBeUpdated)
        {
                EventDTO updatedEvent = await _eventService.UpdateAsync(eventToBeUpdated);

                return Ok(updatedEvent);
        }

        // DELETE api/Events/{id}
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            EventDTO eventToBeDeleted = await _eventService.DeleteAsync(id);
            return Ok(eventToBeDeleted);
        }

        //Filter http methods to be done...
    }
}
