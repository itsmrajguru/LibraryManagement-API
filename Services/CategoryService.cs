using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    // Handles all category operations using the database
    public class CategoryService : ICategoryService
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(LibraryDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<CategoryDto> GetAllCategories()
        {
            return _context.Categories
                .Include(c => c.Books)
                .Select(c => MapToDto(c))
                .ToList();
        }

        public CategoryDto? GetCategoryById(int id)
        {
            var category = _context.Categories.Include(c => c.Books).FirstOrDefault(c => c.Id == id);
            return category == null ? null : MapToDto(category);
        }

        public CategoryDto CreateCategory(CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name, Description = dto.Description };
            _context.Categories.Add(category);
            _context.SaveChanges();
            _logger.LogInformation("Category created: {Name}", category.Name);
            return MapToDto(category);
        }

        public CategoryDto? UpdateCategory(int id, CreateCategoryDto dto)
        {
            var category = _context.Categories.Include(c => c.Books).FirstOrDefault(c => c.Id == id);
            if (category == null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;
            _context.SaveChanges();
            return MapToDto(category);
        }

        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        // Map Category entity to CategoryDto
        private static CategoryDto MapToDto(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            TotalBooks = c.Books.Count
        };
    }
}
