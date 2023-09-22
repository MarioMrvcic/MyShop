using MyShop.Entities;

namespace MyShop.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync();

    Task<Product?> GetProductByIdAsync(int id);

    Task<Product> AddProduct(Product product);

    Task<Product> UpdateProduct(int id, Product product);

    Task<bool> DeleteProduct(int id);

    Task<List<Product>> GetProductByCategory(int categoryId);
}
