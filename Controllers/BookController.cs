using Microsoft.AspNetCore.Mvc;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;

namespace LibraryManagementAPI.Controllers
{
    // Controller: Handles HTTP only. Receives request → calls service → returns response.
    // No business logic here. Think of it as a waiter, not the chef.
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        // Constructor Injection: ASP.NET auto-provides BookService here (via DI)
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }


        [HttpGet]                      // GET /api/book
        public IActionResult GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("available")]         // GET /api/book/available
        public IActionResult GetAvailableBooks()
        {
            return Ok(_bookService.GetAvailableBooks());
        }

        [HttpGet("issued")]            // GET /api/book/issued
        public IActionResult GetIssuedBooks()
        {
            return Ok(_bookService.GetIssuedBooks());
        }

        [HttpGet("{id}")]              // GET /api/book/1
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound(new { message = $"Book with ID {id} not found!" });
            return Ok(book);
        }


        // POST /api/book 
        [HttpPost]
        public IActionResult AddBook([FromBody] BookRequestDto bookDto)
        {
            var newBook = _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook); // 201 Created
        }

        // PUT /api/book/1
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookRequestDto bookDto)
        {
            var updatedBook = _bookService.UpdateBook(id, bookDto);
            if (updatedBook == null) return NotFound(new { message = $"Book with ID {id} not found!" });
            return Ok(new { message = "Book updated successfully!", book = updatedBook });
        }

        // DELETE /api/book/1
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null) return NotFound(new { message = $"Book with ID {id} not found!" });
            if (book.IsIssued) return BadRequest(new { message = $"Cannot delete! Book is issued to {book.IssuedTo}." });

            _bookService.DeleteBook(id);
            return NoContent(); // 204 — standard response for DELETE
        }


        // POST /api/book/1/issue?studentName=Mangesh
        [HttpPost("{id}/issue")]
        public IActionResult IssueBook(int id, [FromQuery] string studentName)
        {
            var (success, message, book) = _bookService.IssueBook(id, studentName);
            if (!success)
            {
                if (message.Contains("not found")) return NotFound(new { message });
                return BadRequest(new { message });
            }
            return Ok(new { message, book });
        }

        // POST /api/book/1/return
        [HttpPost("{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            var (success, message, book) = _bookService.ReturnBook(id);
            if (!success)
            {
                if (message.Contains("not found")) return NotFound(new { message });
                return BadRequest(new { message });
            }
            return Ok(new { message, book });
        }
    }
}