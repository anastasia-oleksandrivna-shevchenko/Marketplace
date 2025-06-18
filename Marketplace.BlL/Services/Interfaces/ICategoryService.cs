using Marketplace.BLL.DTO.Category;

namespace Marketplace.BLL.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesSortedByNameAsync(bool ascending = true);
    Task UpdateCategoryAsync(UpdateCategoryDto dto);
    Task DeleteCategoryAsync(int id);
}