using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherRepository _teacherRepository;

    public TeachersController(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _teacherRepository.GetAllAsync();
        return Ok(teachers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }
        return Ok(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher(Teacher teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _teacherRepository.AddAsync(teacher);
        return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(int id, Teacher teacher)
    {
        if (id != teacher.Id)
        {
            return BadRequest();
        }

        await _teacherRepository.UpdateAsync(teacher);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        await _teacherRepository.DeleteAsync(id);
        return NoContent();
    }
}