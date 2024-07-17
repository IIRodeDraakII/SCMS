public class Library
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<LibraryItem> LibraryItems { get; set; }
}