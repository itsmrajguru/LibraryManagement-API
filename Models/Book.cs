namespace LibraryManagementAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public bool IsIssued { get; set; } = false;  // is book currently with someone?
        public string IssuedTo { get; set; } = "";   // student name who has the book
        public DateTime? IssuedOn { get; set; } = null; // null = not issued, else = issue date
    }
}