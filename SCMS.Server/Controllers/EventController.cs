using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;

    public EventsController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _eventRepository.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(id);
        if (eventEntity == null)
        {
            return NotFound();
        }
        return Ok(eventEntity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(Event eventEntity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _eventRepository.AddAsync(eventEntity);
        return CreatedAtAction(nameof(GetEvent), new { id = eventEntity.Id }, eventEntity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event eventEntity)
    {
        if (id != eventEntity.Id)
        {
            return BadRequest();
        }

        await _eventRepository.UpdateAsync(eventEntity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await _eventRepository.DeleteAsync(id);
        return NoContent();
    }
}