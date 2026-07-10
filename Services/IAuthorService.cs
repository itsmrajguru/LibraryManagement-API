using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    // Contract for all author-related operations
    public interface IAuthorService
    {
        List<AuthorDto> GetAllAuthors();
        AuthorDto? GetAuthorById(int id);
        List<BookDto> GetBooksByAuthor(int authorId);
        AuthorDto CreateAuthor(CreateAuthorDto dto);
        AuthorDto? UpdateAuthor(int id, CreateAuthorDto dto);
        bool DeleteAuthor(int id);
    }
}
