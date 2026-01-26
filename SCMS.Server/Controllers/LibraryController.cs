using Microsoft.AspNetCore.Mvc;
using SCMS.Server.DTOs;

[ApiController]
[Route("api/[controller]")]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryCheckoutRepository _checkoutRepository;

    public LibrariesController(ILibraryRepository libraryRepository, ILibraryCheckoutRepository checkoutRepository)
    {
        _libraryRepository = libraryRepository;
        _checkoutRepository = checkoutRepository;
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

    [HttpPost("items/{itemId}/checkout")]
    public async Task<IActionResult> CheckoutItem(int itemId, [FromBody] CheckoutRequest request)
    {
        try
        {
            var loanDays = request.LoanDays ?? 14;
            var checkout = await _checkoutRepository.CheckoutAsync(itemId, request.StudentId, loanDays);
            return Ok(checkout);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("items/{itemId}/return")]
    public async Task<IActionResult> ReturnItem(int itemId)
    {
        try
        {
            var checkout = await _checkoutRepository.ReturnAsync(itemId);
            return Ok(checkout);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("items/{itemId}/history")]
    public async Task<IActionResult> GetCheckoutHistory(int itemId)
    {
        var history = await _checkoutRepository.GetCheckoutHistoryAsync(itemId);
        return Ok(history);
    }
}
