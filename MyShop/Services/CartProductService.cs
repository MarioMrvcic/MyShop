using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Interfaces;

namespace MyShop.Services;

public class CartProductService : ICartProductService
{

    private readonly IDbContextFactory<AplicationDbContext> _dbContextFactory;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;

    public CartProductService(IDbContextFactory<AplicationDbContext> dbContextFactory, IProductService productService, IUserService userService, ICartService cartService)
    {
        _dbContextFactory = dbContextFactory;
        _productService = productService;
        _userService = userService;
        _cartService = cartService;
    }

    public async Task<CartProduct> GetCartProductIfProductExistsInCart(int cartId, int productId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var returnCartProduct = await dbContext.CartProduct.FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.ProductsId == productId);

        return returnCartProduct;
    }

    public async Task<CartProduct> IncreaseQuantityForProductInCart(CartProduct productToIncrease)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var fetchedCartProduct = await dbContext.CartProduct.Where(cp => cp.ProductsId == productToIncrease.ProductsId && cp.CartId == productToIncrease.CartId).FirstOrDefaultAsync();
        fetchedCartProduct.ProductQuantity++;

        await dbContext.SaveChangesAsync();

        return fetchedCartProduct;
    }

    public async Task<CartProduct> DecreaseQuantityForProductInCart(CartProduct productToDecrease)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var fetchedCartProduct = await dbContext.CartProduct.Where(cp => cp.ProductsId == productToDecrease.ProductsId && cp.CartId == productToDecrease.CartId).FirstOrDefaultAsync();
        fetchedCartProduct.ProductQuantity--;

        if (fetchedCartProduct.ProductQuantity <= 0)
        {
            dbContext.CartProduct.Remove(fetchedCartProduct);
        }

        await dbContext.SaveChangesAsync();

        return fetchedCartProduct;
    }

    public async Task<CartProduct> UpdateQuantityForProductInCart(CartProduct productToUpdate)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var fetchedCartProduct = await dbContext.CartProduct.Where(cp => cp.ProductsId == productToUpdate.ProductsId && cp.CartId == productToUpdate.CartId).FirstOrDefaultAsync();
        fetchedCartProduct.ProductQuantity=productToUpdate.ProductQuantity;

        if (fetchedCartProduct.ProductQuantity <= 0)
        {
            dbContext.CartProduct.Remove(fetchedCartProduct);
        }

        await dbContext.SaveChangesAsync();

        return fetchedCartProduct;
    }

    public async Task<CartProduct> AddProductToCartProduct(int productId, string userName)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var user = await _userService.GetUserByNameAsync(userName);
        var cart = await _cartService.GetUserCurrentCart(user);
        var cartId = cart.Id;
        var fetchetCartProduct = await GetCartProductIfProductExistsInCart(cartId, productId);

        if (fetchetCartProduct != null)
        {
            var increasingExistingProductQuantity = await IncreaseQuantityForProductInCart(fetchetCartProduct);
            return increasingExistingProductQuantity;
        }

        CartProduct cartProduct = new();
        cartProduct.ProductsId = productId;
        cartProduct.CartId = cartId;
        cartProduct.ProductQuantity = 1;
        //cartProduct.Product = await _productService.GetProductByIdAsync(productId);
        //cartProduct.Cart = cart;

        if (cartId != null)
        {
            var cartProductConnection = await dbContext.CartProduct.AddAsync(cartProduct);
            await dbContext.SaveChangesAsync();

            return cartProduct;
        }

        return null;
    }

    public async Task<List<CartProduct>> GetAllCartProductEntriesForCartAsync(Cart cart)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();

        var returnListOfEntries = await dbContext.CartProduct.Where(cp => cp.Cart == cart).ToListAsync();

        return returnListOfEntries;
    }
}
