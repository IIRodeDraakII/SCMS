public class LibraryCheckout
{
    public int Id { get; set; }
    public int LibraryItemId { get; set; }
    public int StudentId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LibraryItem LibraryItem { get; set; }
    public Student Student { get; set; }
}
