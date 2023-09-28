using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICartService
{
    Task<List<Cart>> GetAllCarts();

    Task<Cart> CreateNewCart(string shopAppWebUserId);

    Task<Cart> GetUserCurrentCart(ShopAppWebUser user);

    Task<decimal> SaveCartSum(Cart cartNewValue);

    Task<Cart> LockCart(Cart cartToLock);

    Task<List<Cart>> GetAllUserPastCarts(string userId);
}
