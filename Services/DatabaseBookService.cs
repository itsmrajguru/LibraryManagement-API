using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Data;

namespace LibraryManagementAPI.Services
{
    // Real Database Service: Contains logic to interact with MySQL via EF Core.
    // Implements IBookService, so it can easily replace BookService.
    public class DatabaseBookService : IBookService
    {
        private readonly LibraryDbContext _context;

        // Inject the DbContext
        public DatabaseBookService(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();

        public Book? GetBookById(int id) => _context.Books.Find(id);

        public List<Book> GetAvailableBooks() => _context.Books.Where(b => !b.IsIssued).ToList();

        public List<Book> GetIssuedBooks() => _context.Books.Where(b => b.IsIssued).ToList();

        public Book AddBook(BookRequestDto bookDto)
        {
            var newBook = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author
            };
            
            // Add to database and save
            _context.Books.Add(newBook);
            _context.SaveChanges();
            
            return newBook; // EF Core automatically populates the auto-incremented Id
        }

        public Book? UpdateBook(int id, BookRequestDto bookDto)
        {
            var book = _context.Books.Find(id);
            if (book == null) return null;

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            
            _context.SaveChanges();
            return book;
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null || book.IsIssued) return false;

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }

        public (bool success, string message, Book? book) IssueBook(int id, string studentName)
        {
            var book = _context.Books.Find(id);
            if (book == null) return (false, "Book not found!", null);
            if (book.IsIssued) return (false, $"Book already issued to {book.IssuedTo}!", null);

            book.IsIssued = true;
            book.IssuedTo = studentName;
            book.IssuedOn = DateTime.Now;
            
            _context.SaveChanges();
            return (true, $"Book issued to {studentName}!", book);
        }

        public (bool success, string message, Book? book) ReturnBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return (false, "Book not found!", null);
            if (!book.IsIssued) return (false, "Book was not issued, can't return!", null);

            book.IsIssued = false;
            book.IssuedTo = "";
            book.IssuedOn = null;
            
            _context.SaveChanges();
            return (true, "Book returned successfully!", book);
        }
    }
}
