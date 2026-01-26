public interface ILibraryCheckoutRepository
{
    Task<LibraryCheckout> CheckoutAsync(int itemId, int studentId, int loanDays = 14);
    Task<LibraryCheckout> ReturnAsync(int itemId);
    Task<IEnumerable<LibraryCheckout>> GetCheckoutHistoryAsync(int itemId);
    Task<IEnumerable<LibraryCheckout>> GetStudentCheckoutsAsync(int studentId, bool activeOnly = false);
    Task<LibraryCheckout> GetActiveCheckoutAsync(int itemId);
}
