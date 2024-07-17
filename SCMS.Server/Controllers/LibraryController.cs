using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryRepository _libraryRepository;

    public LibrariesController(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetLibraries()
    {
        var libraries = await _libraryRepository.GetAllAsync();
        return Ok(libraries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLibrary(int id)
    {
        var library = await _libraryRepository.GetByIdAsync(id);
        if (library == null)
        {
            return NotFound();
        }
        return Ok(library);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLibrary(Library library)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _libraryRepository.AddAsync(library);
        return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, library);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLibrary(int id, Library library)
    {
        if (id != library.Id)
        {
            return BadRequest();
        }

        await _libraryRepository.UpdateAsync(library);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibrary(int id)
    {
        Library library = await _libraryRepository.GetByIdAsync(id);
        await _libraryRepository.DeleteAsync(library);
        return NoContent();
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetLibraryItems(int id)
    {
        var libraryItems = await _libraryRepository.GetLibraryItemsAsync(id);
        return Ok(libraryItems);
    }
}
