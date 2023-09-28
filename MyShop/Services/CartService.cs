using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class CartService : ICartService
{
    private readonly IDbContextFactory<AplicationDbContext> _dbContextFactory;

    public CartService(IDbContextFactory<AplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    [Inject]
    public IUserService UserService { get; set; }
    [Inject]
    public IProductService ProductService { get; set; }

    public async Task<List<Cart>> GetAllCarts()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var carts = await dbContext.Carts.ToListAsync();
        return carts;
    }

    public async Task<Cart> CreateNewCart(string shopAppWebUserId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        Cart cart = new();
        cart.CartStatus = CartStatus.INPROGRESS;
        cart.ShopAppWebUserId = shopAppWebUserId;

        await dbContext.Carts.AddAsync(cart);
        await dbContext.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> GetUserCurrentCart(ShopAppWebUser user)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var cart = await dbContext.Carts.SingleOrDefaultAsync(c => c.ShopAppWebUser == user && c.CartStatus == 0);

        return cart;
    }

    public async Task<decimal> SaveCartSum(Cart cartNewValue)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var cartToUpdate = await dbContext.Carts.SingleOrDefaultAsync(c => c.Id == cartNewValue.Id);
        cartToUpdate.CartCost = cartNewValue.CartCost;

        await dbContext.SaveChangesAsync();

        return cartNewValue.CartCost;
    }

    public async Task<Cart> LockCart(Cart cartToLock)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var cartToUpdate = await dbContext.Carts.SingleOrDefaultAsync(c => c.Id == cartToLock.Id);
        cartToUpdate.CartStatus = CartStatus.FINISHED;
        cartToUpdate.FinishedCartOnDate = DateTime.Now;

        await dbContext.SaveChangesAsync();

        return cartToUpdate;
    }

    public async Task<List<Cart>> GetAllUserPastCarts(string userId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var allPastUserCarts = await dbContext.Carts.Where(c => c.ShopAppWebUserId == userId && c.CartStatus == CartStatus.FINISHED).ToListAsync();

        return allPastUserCarts;
    }
}
