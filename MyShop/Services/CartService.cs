using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class CartService : ICartService
{
    private readonly AplicationDbContext _aplicationdbcontext;

    public CartService(AplicationDbContext aplicationdbcontext)
    {
        _aplicationdbcontext = aplicationdbcontext;
    }

    public async Task<List<Cart>> GetCarts()
    {
        var carts = await _aplicationdbcontext.Carts.ToListAsync();
        return carts;
    }

    public async Task<Cart> CreateNewCart(ShopAppWebUser shopAppWebUser)
    {
        Cart cart = new Cart();
        cart.CartStatus = CartStatus.INPROGRESS;
        cart.ShopAppWebUser = shopAppWebUser;

        await _aplicationdbcontext.Carts.AddAsync(cart);
        await _aplicationdbcontext.SaveChangesAsync();

        return cart;
    }

    public async Task<bool> AddProductToCart(int productId, string userName)
    {
        var userId = await _aplicationdbcontext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        var cartToAddIn = await _aplicationdbcontext.Carts.SingleOrDefaultAsync(u => u.ShopAppWebUser == userId);
        var cartId = cartToAddIn.Id;

        if (cartToAddIn != null)
        {


            return true;
        }

        return false;
    }
}
