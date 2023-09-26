using MyShop.Entities;

namespace MyShop.Interfaces;

public interface IUserService
{
    Task<ShopAppWebUser> GetUserByNameAsync(string userName);
}
