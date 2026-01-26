using Microsoft.EntityFrameworkCore;

namespace Server.Repositories;

public class LibraryCheckoutRepository : ILibraryCheckoutRepository
{
    private readonly SchoolContext _context;

    public LibraryCheckoutRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<LibraryCheckout> CheckoutAsync(int itemId, int studentId, int loanDays = 14)
    {
        var item = await _context.LibraryItems.FindAsync(itemId);
        if (item == null)
        {
            throw new InvalidOperationException($"Library item with ID {itemId} not found.");
        }

        if (item.IsCheckedOut)
        {
            throw new InvalidOperationException($"Library item with ID {itemId} is already checked out.");
        }

        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID {studentId} not found.");
        }

        var checkout = new LibraryCheckout
        {
            LibraryItemId = itemId,
            StudentId = studentId,
            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(loanDays),
            ReturnDate = null
        };

        item.IsCheckedOut = true;

        await _context.LibraryCheckouts.AddAsync(checkout);
        await _context.SaveChangesAsync();

        return checkout;
    }

    public async Task<LibraryCheckout> ReturnAsync(int itemId)
    {
        var item = await _context.LibraryItems.FindAsync(itemId);
        if (item == null)
        {
            throw new InvalidOperationException($"Library item with ID {itemId} not found.");
        }

        var activeCheckout = await GetActiveCheckoutAsync(itemId);
        if (activeCheckout == null)
        {
            throw new InvalidOperationException($"Library item with ID {itemId} is not currently checked out.");
        }

        activeCheckout.ReturnDate = DateTime.UtcNow;
        item.IsCheckedOut = false;

        await _context.SaveChangesAsync();

        return activeCheckout;
    }

    public async Task<IEnumerable<LibraryCheckout>> GetCheckoutHistoryAsync(int itemId)
    {
        return await _context.LibraryCheckouts
            .Include(c => c.Student)
            .Include(c => c.LibraryItem)
            .Where(c => c.LibraryItemId == itemId)
            .OrderByDescending(c => c.CheckoutDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LibraryCheckout>> GetStudentCheckoutsAsync(int studentId, bool activeOnly = false)
    {
        var query = _context.LibraryCheckouts
            .Include(c => c.LibraryItem)
            .Where(c => c.StudentId == studentId);

        if (activeOnly)
        {
            query = query.Where(c => c.ReturnDate == null);
        }

        return await query.OrderByDescending(c => c.CheckoutDate).ToListAsync();
    }

    public async Task<LibraryCheckout> GetActiveCheckoutAsync(int itemId)
    {
        return await _context.LibraryCheckouts
            .Include(c => c.Student)
            .Include(c => c.LibraryItem)
            .FirstOrDefaultAsync(c => c.LibraryItemId == itemId && c.ReturnDate == null);
    }
}
