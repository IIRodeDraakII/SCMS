using Microsoft.EntityFrameworkCore;
using Server.Repositories;

namespace ServerTests;

public class LibraryCheckoutRepositoryTests : IDisposable
{
    private readonly SchoolContext _context;
    private readonly LibraryCheckoutRepository _repository;

    public LibraryCheckoutRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SchoolContext(options);
        _repository = new LibraryCheckoutRepository(_context);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var library = new Library { Id = 1, Name = "Main Library" };
        var student = new Student { Id = 1, Name = "John Doe", DateOfBirth = DateTime.UtcNow.AddYears(-15), Age = 15, Grade = "10th" };
        var libraryItem = new LibraryItem { Id = 1, Title = "Test Book", Author = "Test Author", IsCheckedOut = false, LibraryId = 1 };

        _context.Libraries.Add(library);
        _context.Students.Add(student);
        _context.LibraryItems.Add(libraryItem);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CheckoutAsync_ValidItemAndStudent_CreatesCheckout()
    {
        var checkout = await _repository.CheckoutAsync(1, 1, 14);

        Assert.NotNull(checkout);
        Assert.Equal(1, checkout.LibraryItemId);
        Assert.Equal(1, checkout.StudentId);
        Assert.Null(checkout.ReturnDate);

        var item = await _context.LibraryItems.FindAsync(1);
        Assert.True(item.IsCheckedOut);
    }

    [Fact]
    public async Task CheckoutAsync_ItemNotFound_ThrowsInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.CheckoutAsync(999, 1, 14));
    }

    [Fact]
    public async Task CheckoutAsync_StudentNotFound_ThrowsInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.CheckoutAsync(1, 999, 14));
    }

    [Fact]
    public async Task CheckoutAsync_ItemAlreadyCheckedOut_ThrowsInvalidOperationException()
    {
        await _repository.CheckoutAsync(1, 1, 14);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.CheckoutAsync(1, 1, 14));
    }

    [Fact]
    public async Task ReturnAsync_ValidCheckout_SetsReturnDate()
    {
        await _repository.CheckoutAsync(1, 1, 14);

        var returnedCheckout = await _repository.ReturnAsync(1);

        Assert.NotNull(returnedCheckout);
        Assert.NotNull(returnedCheckout.ReturnDate);

        var item = await _context.LibraryItems.FindAsync(1);
        Assert.False(item.IsCheckedOut);
    }

    [Fact]
    public async Task ReturnAsync_ItemNotCheckedOut_ThrowsInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.ReturnAsync(1));
    }

    [Fact]
    public async Task ReturnAsync_ItemNotFound_ThrowsInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _repository.ReturnAsync(999));
    }

    [Fact]
    public async Task GetCheckoutHistoryAsync_ReturnsAllCheckoutsForItem()
    {
        await _repository.CheckoutAsync(1, 1, 14);
        await _repository.ReturnAsync(1);
        await _repository.CheckoutAsync(1, 1, 14);

        var history = await _repository.GetCheckoutHistoryAsync(1);

        Assert.Equal(2, history.Count());
    }

    [Fact]
    public async Task GetStudentCheckoutsAsync_ReturnsAllCheckoutsForStudent()
    {
        var item2 = new LibraryItem { Id = 2, Title = "Another Book", Author = "Another Author", IsCheckedOut = false, LibraryId = 1 };
        _context.LibraryItems.Add(item2);
        await _context.SaveChangesAsync();

        await _repository.CheckoutAsync(1, 1, 14);
        await _repository.CheckoutAsync(2, 1, 14);

        var checkouts = await _repository.GetStudentCheckoutsAsync(1, activeOnly: false);

        Assert.Equal(2, checkouts.Count());
    }

    [Fact]
    public async Task GetStudentCheckoutsAsync_ActiveOnly_ReturnsOnlyActiveCheckouts()
    {
        var item2 = new LibraryItem { Id = 2, Title = "Another Book", Author = "Another Author", IsCheckedOut = false, LibraryId = 1 };
        _context.LibraryItems.Add(item2);
        await _context.SaveChangesAsync();

        await _repository.CheckoutAsync(1, 1, 14);
        await _repository.ReturnAsync(1);
        await _repository.CheckoutAsync(2, 1, 14);

        var activeCheckouts = await _repository.GetStudentCheckoutsAsync(1, activeOnly: true);

        Assert.Single(activeCheckouts);
        Assert.Equal(2, activeCheckouts.First().LibraryItemId);
    }

    [Fact]
    public async Task GetActiveCheckoutAsync_ReturnsActiveCheckout()
    {
        await _repository.CheckoutAsync(1, 1, 14);

        var activeCheckout = await _repository.GetActiveCheckoutAsync(1);

        Assert.NotNull(activeCheckout);
        Assert.Null(activeCheckout.ReturnDate);
    }

    [Fact]
    public async Task GetActiveCheckoutAsync_NoActiveCheckout_ReturnsNull()
    {
        var activeCheckout = await _repository.GetActiveCheckoutAsync(1);

        Assert.Null(activeCheckout);
    }
}
