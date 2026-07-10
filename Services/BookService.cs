using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    // Service Layer: Contains all business logic.
    // Controller just calls these methods — it doesn't think, it just delegates.
    public class BookService : IBookService
    {
        // In-memory "fake DB". static = shared across all requests so data persists.
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert Martin" },
            new Book { Id = 2, Title = "Annihilation of Caste", Author = "B.R. Ambedkar" }
        };

        public List<Book> GetAllBooks() => _books;

        public Book? GetBookById(int id) => _books.FirstOrDefault(b => b.Id == id);

        // LINQ Where() filters the list; ToList() converts result back to List<Book>
        public List<Book> GetAvailableBooks() => _books.Where(b => !b.IsIssued).ToList();

        public List<Book> GetIssuedBooks() => _books.Where(b => b.IsIssued).ToList();

        public Book AddBook(BookRequestDto bookDto)
        {
            var newBook = new Book
            {
                Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1,
                Title = bookDto.Title,
                Author = bookDto.Author
            };
            _books.Add(newBook);
            return newBook;
        }

        public Book? UpdateBook(int id, BookRequestDto bookDto)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return null;

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            return book;
        }

        public bool DeleteBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null || book.IsIssued) return false;

            _books.Remove(book);
            return true;
        }

        // Returns a Tuple: (success flag, message, book object)
        public (bool success, string message, Book? book) IssueBook(int id, string studentName)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return (false, "Book not found!", null);
            if (book.IsIssued) return (false, $"Book already issued to {book.IssuedTo}!", null);

            book.IsIssued = true;
            book.IssuedTo = studentName;
            book.IssuedOn = DateTime.Now; // record the exact issue date & time
            return (true, $"Book issued to {studentName}!", book);
        }

        public (bool success, string message, Book? book) ReturnBook(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return (false, "Book not found!", null);
            if (!book.IsIssued) return (false, "Book was not issued, can't return!", null);

            book.IsIssued = false;
            book.IssuedTo = "";
            book.IssuedOn = null; // reset date on return
            return (true, "Book returned successfully!", book);
        }
    }
}
