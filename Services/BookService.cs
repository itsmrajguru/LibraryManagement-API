using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // In-memory fake service — kept for reference only, DatabaseBookService is used in production.
    // This class is NOT registered in DI — it's just here so the project remembers how we started.
    public class BookService : IBookService
    {
        private static readonly List<(int Id, string Title, string AuthorName, int CategoryId, int Total, int Available)> _books = new();

        public List<BookDto> GetAllBooks()
            => _books
                .Select(b => new BookDto { Id = b.Id, Title = b.Title, AuthorName = b.AuthorName, TotalCopies = b.Total, AvailableCopies = b.Available })
                .ToList();

        public BookDto? GetBookById(int id)
        {
            var b = _books.FirstOrDefault(x => x.Id == id);
            return b == default ? null : new BookDto { Id = b.Id, Title = b.Title, AuthorName = b.AuthorName };
        }

        public BookDto AddBook(CreateBookDto dto)
        {
            var id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add((id, dto.Title, dto.AuthorName, dto.CategoryId, dto.TotalCopies, dto.TotalCopies));
            return new BookDto { Id = id, Title = dto.Title, AuthorName = dto.AuthorName };
        }

        public BookDto? UpdateBook(int id, UpdateBookDto dto)
        {
            var idx = _books.FindIndex(b => b.Id == id);
            if (idx < 0) return null;
            _books[idx] = (id, dto.Title, dto.AuthorName, dto.CategoryId, dto.TotalCopies, _books[idx].Available);
            return new BookDto { Id = id, Title = dto.Title };
        }

        public bool DeleteBook(int id)
        {
            var b = _books.FirstOrDefault(x => x.Id == id);
            if (b == default) return false;
            _books.Remove(b);
            return true;
        }

        public object GetAvailability(int id)
        {
            var b = _books.FirstOrDefault(x => x.Id == id);
            return b == default ? new { message = "Not found" } : new { bookId = b.Id, availableCopies = b.Available };
        }
    }
}
