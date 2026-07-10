using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends when creating an author
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Author name is required!")]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(500)]
        public string Bio { get; set; } = "";
    }

    // What we send back to the client
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Bio { get; set; } = "";
        public int TotalBooks { get; set; }
    }
}
