using LibraryManagementAPI.Models.Enums;

namespace LibraryManagementAPI.Models
{
    // Author of a book — one author can write many books
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Bio { get; set; } = "";

        // Navigation: all books written by this author
        public List<Book> Books { get; set; } = new();
    }
}
