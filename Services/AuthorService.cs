using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Handles all author operations using the database
    public class AuthorService : IAuthorService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(LibraryDbContext context, ILogger<AuthorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<AuthorDto> GetAllAuthors()
        {
            return _context.Authors
                .Include(a => a.Books)
                .Select(a => MapToDto(a))
                .ToList();
        }

        public AuthorDto? GetAuthorById(int id)
        {
            var author = _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);
            return author == null ? null : MapToDto(author);
        }

        public List<BookDto> GetBooksByAuthor(int authorId)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.AuthorId == authorId)
                .Select(b => MapBookToDto(b))
                .ToList();
        }

        public AuthorDto CreateAuthor(CreateAuthorDto dto)
        {
            var author = new Author { Name = dto.Name, Bio = dto.Bio };
            _context.Authors.Add(author);
            _context.SaveChanges();
            _logger.LogInformation("Author created: {Name}", author.Name);
            return MapToDto(author);
        }

        public AuthorDto? UpdateAuthor(int id, CreateAuthorDto dto)
        {
            var author = _context.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);
            if (author == null) return null;

            author.Name = dto.Name;
            author.Bio = dto.Bio;
            _context.SaveChanges();
            return MapToDto(author);
        }

        public bool DeleteAuthor(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            _context.SaveChanges();
            return true;
        }

        // Map Author entity to AuthorDto
        private static AuthorDto MapToDto(Author a) => new()
        {
            Id = a.Id,
            Name = a.Name,
            Bio = a.Bio,
            TotalBooks = a.Books.Count
        };

        // Map Book entity to BookDto
        private static BookDto MapBookToDto(Book b) => new()
        {
            Id = b.Id,
            Title = b.Title,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies,
            AuthorName = b.Author?.Name ?? "",
            CategoryName = b.Category?.Name ?? ""
        };
    }
}
