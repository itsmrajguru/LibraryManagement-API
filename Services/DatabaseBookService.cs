using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Real Database Service: Contains logic to interact with MySQL via EF Core.
    // Implements IBookService, so it can easily replace BookService.
    public class DatabaseBookService : IBookService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<DatabaseBookService> _logger;

        // Inject the DbContext
        public DatabaseBookService(LibraryDbContext context, ILogger<DatabaseBookService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all books from the database
        public List<BookDto> GetAllBooks()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => MapToDto(b))
                .ToList();
        }

        public BookDto? GetBookById(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            return book == null ? null : MapToDto(book);
        }

        public BookDto AddBook(CreateBookDto dto)
        {
            // Find author by name or create a new one
            var author = _context.Authors.FirstOrDefault(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName, Bio = "Auto-created author" };
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            var book = new Book
            {
                Title = dto.Title,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,
                AuthorId = author.Id,
                CategoryId = dto.CategoryId
            };

            _context.Books.Add(book);
            _context.SaveChanges();
            _logger.LogInformation("Book added: {Title}", book.Title);

            // Reload with navigation properties to return full DTO
            return GetBookById(book.Id)!;
        }

        public BookDto? UpdateBook(int id, UpdateBookDto dto)
        {
            var book = _context.Books.Find(id);
            if (book == null) return null;

            var author = _context.Authors.FirstOrDefault(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName, Bio = "Auto-created author" };
                _context.Authors.Add(author);
                _context.SaveChanges();
            }

            book.Title = dto.Title;
            book.TotalCopies = dto.TotalCopies;
            book.AuthorId = author.Id;
            book.CategoryId = dto.CategoryId;

            _context.SaveChanges();
            return GetBookById(id);
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            _context.SaveChanges();
            _logger.LogInformation("Book deleted: ID {Id}", id);
            return true;
        }

        // Check how many copies are available vs total
        public object GetAvailability(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return new { message = "Book not found!" };

            return new
            {
                bookId = book.Id,
                title = book.Title,
                totalCopies = book.TotalCopies,
                availableCopies = book.AvailableCopies,
                isAvailable = book.AvailableCopies > 0
            };
        }

        // Map Book entity to BookDto
        private static BookDto MapToDto(Book b) => new()
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
