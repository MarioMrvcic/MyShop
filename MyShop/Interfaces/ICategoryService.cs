using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetCategoriesAsync();

    Task<Category?> GetCategoryByIdAsync(int id);

    Task<Category> AddCategory(Category category);

    Task<Category> UpdateCategory(int id, Category category);

    Task<bool> DeleteCategory(int id);

    Task<bool> CheckCategoryName(string name);

    Task<bool> CheckCategoryDisplayOrder(int diplayOrder);
}
