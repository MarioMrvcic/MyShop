using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICartService
{
    Task<List<Cart>> GetAllCarts();

    Task<Cart> CreateNewCart(ShopAppWebUser shopAppWebUser);

    Task<Cart> GetUserCurrentCart(ShopAppWebUser user);
}
