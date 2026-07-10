namespace LibraryManagementAPI.Models
{
    // Represents a book in the library
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;

        // FK: which author wrote this book
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        // FK: which category this book belongs to
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Navigation: all issue records for this book
        public List<Issue> Issues { get; set; } = new();
    }
}