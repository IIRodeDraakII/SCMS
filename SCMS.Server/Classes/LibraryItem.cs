public class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsCheckedOut { get; set; }
    public int LibraryId { get; set; }
    public Library Library { get; set; }
}