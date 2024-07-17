using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILibraryRepository
{
    Task<Library> GetByIdAsync(int id);
    Task<IEnumerable<Library>> GetAllAsync();
    Task AddAsync(Library library);
    Task UpdateAsync(Library library);
    Task DeleteAsync(Library library);



    Task<LibraryItem> GetLibraryItemByIdAsync(int id);
    Task<IEnumerable<LibraryItem>> GetAllLibraryItemsAsync();
    Task<IEnumerable<LibraryItem>> GetLibraryItemsAsync(int libraryId);
    Task AddLibraryItemAsync(LibraryItem libraryItem);
    Task UpdateLibraryItemAsync(LibraryItem libraryItem);
    Task DeleteLibraryItemAsync(LibraryItem libraryItem);
}