using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles HTTP requests for author management
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]                         // GET /api/author
        [AllowAnonymous]
        public IActionResult GetAllAuthors()
        {
            return Ok(_authorService.GetAllAuthors());
        }

        [HttpGet("{id}")]                 // GET /api/author/1
        [AllowAnonymous]
        public IActionResult GetAuthorById(int id)
        {
            var author = _authorService.GetAuthorById(id);
            if (author == null) return NotFound(new { message = $"Author with ID {id} not found!" });
            return Ok(author);
        }

        [HttpGet("{id}/books")]           // GET /api/author/1/books
        [AllowAnonymous]
        public IActionResult GetBooksByAuthor(int id)
        {
            return Ok(_authorService.GetBooksByAuthor(id));
        }

        [HttpPost]                        // POST /api/author — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult CreateAuthor([FromBody] CreateAuthorDto dto)
        {
            var author = _authorService.CreateAuthor(dto);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]                 // PUT /api/author/1 — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult UpdateAuthor(int id, [FromBody] CreateAuthorDto dto)
        {
            var author = _authorService.UpdateAuthor(id, dto);
            if (author == null) return NotFound(new { message = $"Author with ID {id} not found!" });
            return Ok(author);
        }

        [HttpDelete("{id}")]              // DELETE /api/author/1 — Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAuthor(int id)
        {
            var success = _authorService.DeleteAuthor(id);
            if (!success) return NotFound(new { message = $"Author with ID {id} not found!" });
            return NoContent();
        }
    }
}
