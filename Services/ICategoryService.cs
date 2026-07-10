using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    // Contract for all category-related operations
    public interface ICategoryService
    {
        List<CategoryDto> GetAllCategories();
        CategoryDto? GetCategoryById(int id);
        CategoryDto CreateCategory(CreateCategoryDto dto);
        CategoryDto? UpdateCategory(int id, CreateCategoryDto dto);
        bool DeleteCategory(int id);
    }
}
