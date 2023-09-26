using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class CategoryService : ICategoryService
{
    private readonly IDbContextFactory<AplicationDbContext> _dbContextFactory;

    public CategoryService(IDbContextFactory<AplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var categories = await dbContext.Categories.ToListAsync();
        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var category = await dbContext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
        return category;

    }

    public async Task<Category> AddCategory(Category category)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        return category;

    }

    public async Task<Category> UpdateCategory(int id, Category category)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var existingCategory = await dbContext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (existingCategory == null)
        {
            throw new Exception("Category not found");
        }

        existingCategory.Name = category.Name;
        existingCategory.DisplayOrder = category.DisplayOrder;

        await dbContext.SaveChangesAsync();

        return category;

    }

    public async Task<bool> DeleteCategory(int id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var categoryToDelete = await dbContext.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (categoryToDelete == null)
        {
            return false;
        }

        dbContext.Categories.Remove(categoryToDelete);

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckCategoryName(string name)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var nameToCheck = await dbContext.Categories.Where(c => c.Name == name).AnyAsync();

        return !nameToCheck;
    }

    public async Task<bool> CheckCategoryDisplayOrder(int diplayOrder)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var displayOrderToCheck = await dbContext.Categories.Where(c => c.DisplayOrder == diplayOrder).AnyAsync();

        return !displayOrderToCheck;
    }
}
