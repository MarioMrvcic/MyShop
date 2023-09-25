using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICartService
{
    Task<List<Cart>> GetCarts();

    Task<Cart> CreateNewCart(ShopAppWebUser shopAppWebUser);

    Task<bool> AddProductToCart(int productId, string userName);
}
