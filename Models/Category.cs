namespace LibraryManagementAPI.Models
{
    // Category/Genre of a book — one category can have many books
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        // Navigation: all books under this category
        public List<Book> Books { get; set; } = new();
    }
}
