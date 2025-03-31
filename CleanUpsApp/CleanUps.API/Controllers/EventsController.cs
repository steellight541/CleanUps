using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CleanUps.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IService<EventDTO> _eventService;

        public EventsController(IService<EventDTO> eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EventDTO dto)
        {
            var result = await _eventService.CreateAsync(dto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Data.EventId }, result.Data);
            }
            return StatusCode(result.StatusCode, new { Error = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _eventService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, new { Error = result.ErrorMessage });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _eventService.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, new { Error = result.ErrorMessage });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] EventDTO dto)
        {
            var result = await _eventService.UpdateAsync(id, dto);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, new { Error = result.ErrorMessage });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _eventService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.StatusCode, new { Error = result.ErrorMessage });
        }
    }
}