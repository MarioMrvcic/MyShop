using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class ProductService : IProductService
{
    private readonly AplicationDbContext _aplicationdbcontext;

    public ProductService(AplicationDbContext aplicationdbcontext)
    {
        _aplicationdbcontext = aplicationdbcontext;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await _aplicationdbcontext.Products.Include((p) => p.Category).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductByCategory(int categoryId)
    {
        var productsWithCategoryId = await _aplicationdbcontext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        return productsWithCategoryId;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var products = await _aplicationdbcontext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();
        return products;
    }

    public async Task<Product> AddProduct(Product product)
    {
        await _aplicationdbcontext.Products.AddAsync(product);
        await _aplicationdbcontext.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProduct(int id, Product product)
    {
        var existingProduct = await _aplicationdbcontext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();

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

        await _aplicationdbcontext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var productToDelete = await _aplicationdbcontext.Products.Where(c => c.Id == id).SingleOrDefaultAsync();

        if (productToDelete == null)
        {
            return false;
        }

        _aplicationdbcontext.Products.Remove(productToDelete);

        await _aplicationdbcontext.SaveChangesAsync();

        return true;
    }
}
