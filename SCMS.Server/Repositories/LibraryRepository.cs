using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Server.Repositories;
public class LibraryRepository : ILibraryRepository
{
    private readonly SchoolContext _context;

    public LibraryRepository(SchoolContext context)
    {
        _context = context;
    }


    public async Task<Library> GetByIdAsync(int id)
    {
        return await _context.Libraries
            .Include(l => l.LibraryItems)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Library>> GetAllAsync()
    {
        return await _context.Libraries
            .Include(l => l.LibraryItems)
            .ToListAsync();
    }

    public async Task AddAsync(Library library)
    {
        await _context.Libraries.AddAsync(library);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Library library)
    {
        _context.Libraries.Update(library);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Library library)
    {
        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
    }


    public async Task<LibraryItem> GetLibraryItemByIdAsync(int id)
    {
        return await _context.LibraryItems.FindAsync(id);
    }

    public async Task<IEnumerable<LibraryItem>> GetAllLibraryItemsAsync()
    {
        return await _context.LibraryItems.ToListAsync();
    }

    public async Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync(int libraryId)
    {
        return await _context.LibraryItems
            .Where(li => li.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task AddLibraryItemAsync(LibraryItem libraryItem)
    {
        await _context.LibraryItems.AddAsync(libraryItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLibraryItemAsync(LibraryItem libraryItem)
    {
        _context.LibraryItems.Update(libraryItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLibraryItemAsync(LibraryItem libraryItem)
    {
        _context.LibraryItems.Remove(libraryItem);
        await _context.SaveChangesAsync();
    }
}