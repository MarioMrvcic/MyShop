using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class CategoryService : ICategoryService
{
    private readonly AplicationDbContext _aplicationdbcontext;

    public CategoryService(AplicationDbContext aplicationdbcontext)
    {
        _aplicationdbcontext = aplicationdbcontext;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await _aplicationdbcontext.Categories.ToListAsync();
        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var category = await _aplicationdbcontext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
        return category;

    }

    public async Task<Category> AddCategory(Category category)
    {
        await _aplicationdbcontext.Categories.AddAsync(category);
        await _aplicationdbcontext.SaveChangesAsync();

        return category;

    }

    public async Task<Category> UpdateCategory(int id, Category category)
    {
        var existingCategory = await _aplicationdbcontext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (existingCategory == null)
        {
            throw new Exception("Category not found");
        }

        existingCategory.Name = category.Name;
        existingCategory.DisplayOrder = category.DisplayOrder;

        await _aplicationdbcontext.SaveChangesAsync();

        return category;

    }

    public async Task<bool> DeleteCategory(int id)
    {
        var categoryToDelete = await _aplicationdbcontext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (categoryToDelete == null)
        {
            return false;
        }

        _aplicationdbcontext.Categories.Remove(categoryToDelete);

        await _aplicationdbcontext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckCategoryName(string name)
    {
        var nameToCheck = await _aplicationdbcontext.Categories.Where(c => c.Name == name).AnyAsync();

        return !nameToCheck;
    }

    public async Task<bool> CheckCategoryDisplayOrder(int diplayOrder)
    {
        var displayOrderToCheck = await _aplicationdbcontext.Categories.Where(c => c.DisplayOrder == diplayOrder).AnyAsync();

        return !displayOrderToCheck;
    }
}
