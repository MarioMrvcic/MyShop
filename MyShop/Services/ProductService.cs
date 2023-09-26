using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class ProductService : IProductService
{
    private readonly IDbContextFactory<AplicationDbContext> _dbContextFactory;

    public ProductService(IDbContextFactory<AplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var products = await dbContext.Products.Include((p) => p.Category).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductByCategory(int categoryId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var productsWithCategoryId = await dbContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        return productsWithCategoryId;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var products = await dbContext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();
        return products;
    }

    public async Task<Product> AddProduct(Product product)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProduct(int id, Product product)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var existingProduct = await dbContext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (existingProduct == null)
        {
            throw new Exception("Product not found");
        }

        existingProduct.Name = product.Name;
        existingProduct.BrandName = product.BrandName;
        existingProduct.Price = product.Price;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.AssociatedImage = product.AssociatedImage;

        await dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var productToDelete = await dbContext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (productToDelete == null)
        {
            return false;
        }

        dbContext.Products.Remove(productToDelete);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
