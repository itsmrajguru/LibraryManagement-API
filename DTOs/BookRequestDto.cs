using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    /*DTO (Data Transfer Object):
    It is nothing but speacially designed class, that contains
    only fields , a developer wants from the user
    so if the user even try to submit, or overrite 
    informtation, other that the asked fields, the request
    will not be acceepted 
    
    it just relplaces manual selection of the fields, in js
    like 
    const{book_name,author}=req.body
    
    here we create a special class that stores book_name
    and author
    and it is automatically implemented...
    
    thus we create diffremt dto's as per need */
    
    public class BookRequestDto
    {
        [Required(ErrorMessage = "Title is required!")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Author name is required!")]
        [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
        public string Author { get; set; } = "";
    }
}
