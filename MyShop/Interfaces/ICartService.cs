using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICartService
{
    Task<List<Cart>> GetAllCarts();

    Task<Cart> CreateNewCart(string shopAppWebUserId);

    Task<Cart> GetUserCurrentCart(ShopAppWebUser user);
}
