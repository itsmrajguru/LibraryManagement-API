using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    // Handles HTTP requests for category management
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]                         // GET /api/category
        [AllowAnonymous]
        public IActionResult GetAllCategories()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpGet("{id}")]                 // GET /api/category/1
        [AllowAnonymous]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound(new { message = $"Category with ID {id} not found!" });
            return Ok(category);
        }

        [HttpPost]                        // POST /api/category — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var category = _categoryService.CreateCategory(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]                 // PUT /api/category/1 — Admin/Librarian only
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto dto)
        {
            var category = _categoryService.UpdateCategory(id, dto);
            if (category == null) return NotFound(new { message = $"Category with ID {id} not found!" });
            return Ok(category);
        }

        [HttpDelete("{id}")]              // DELETE /api/category/1 — Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            var success = _categoryService.DeleteCategory(id);
            if (!success) return NotFound(new { message = $"Category with ID {id} not found!" });
            return NoContent();
        }
    }
}
