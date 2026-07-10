using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends when creating a new book
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required!")]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Author name is required!")]
        public string AuthorName { get; set; } = "";
        [Range(1, 1000, ErrorMessage = "Total copies must be between 1 and 1000.")]
        public int TotalCopies { get; set; } = 1;

        [Required(ErrorMessage = "CategoryId is required!")]
        public int CategoryId { get; set; }
    }

    // What the client sends to update an existing book
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Title is required!")]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Range(1, 1000)]
        public int TotalCopies { get; set; } = 1;

        public string AuthorName { get; set; } = "";
        public int CategoryId { get; set; }
    }

    // What we send back to the client (no sensitive internals)
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string AuthorName { get; set; } = "";
        public string CategoryName { get; set; } = "";
    }
}
