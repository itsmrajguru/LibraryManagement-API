using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;

namespace LibraryManagementAPI.Controllers
{
    // Controller: Handles HTTP only. Receives request → calls service → returns response.
    // No business logic here. Think of it as a waiter, not the chef.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        // Constructor Injection: ASP.NET auto-provides BookService here (via DI)
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]                      // GET /api/book
        [AllowAnonymous]
        public IActionResult GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]                 // GET /api/book/1
        [AllowAnonymous]
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound(new { message = $"Book with ID {id} not found!" });
            return Ok(book);
        }

        [HttpGet("{id}/availability")]    // GET /api/book/1/availability
        [AllowAnonymous]
        public IActionResult GetAvailability(int id)
        {
            return Ok(_bookService.GetAvailability(id));
        }

        [HttpPost]                        // POST /api/book — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult AddBook([FromBody] CreateBookDto dto)
        {
            var newBook = _bookService.AddBook(dto);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id}")]                 // PUT /api/book/1 — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            var updatedBook = _bookService.UpdateBook(id, dto);
            if (updatedBook == null) return NotFound(new { message = $"Book with ID {id} not found!" });
            return Ok(new { message = "Book updated successfully!", book = updatedBook });
        }

        [HttpDelete("{id}")]              // DELETE /api/book/1 — Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound(new { message = $"Book with ID {id} not found!" });

            _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}