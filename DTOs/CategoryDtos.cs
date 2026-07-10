using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    // What the client sends when creating a category
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required!")]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(300)]
        public string Description { get; set; } = "";
    }

    // What we send back to the client
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int TotalBooks { get; set; }
    }
}
