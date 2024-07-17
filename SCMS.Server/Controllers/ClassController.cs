using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IClassRepository _classRepository;

    public ClassesController(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetClasses()
    {
        var classes = await _classRepository.GetAllAsync();
        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
    {
        var classEntity = await _classRepository.GetByIdAsync(id);
        if (classEntity == null)
        {
            return NotFound();
        }
        return Ok(classEntity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClass(Class classEntity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _classRepository.AddAsync(classEntity);
        return CreatedAtAction(nameof(GetClass), new { id = classEntity.Id }, classEntity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, Class classEntity)
    {
        if (id != classEntity.Id)
        {
            return BadRequest();
        }

        await _classRepository.UpdateAsync(classEntity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        await _classRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetClassesByTeacher(int teacherId)
    {
        var classes = await _classRepository.GetClassesByTeacherIdAsync(teacherId);
        return Ok(classes);
    }
}