using MyShop.Entities;

namespace MyShop.Interfaces;

public interface ICartProductService
{
    Task<CartProduct> GetCartProductIfProductExistsInCart(int cartId, int productId);

    Task<CartProduct> IncreaseQuantityForProductInCart(CartProduct productToIncrease);

    Task<CartProduct> DecreaseQuantityForProductInCart(CartProduct productToDecrease);

    Task<CartProduct> AddProductToCartProduct(int productId, string userName);

    Task<List<CartProduct>> GetAllCartProductEntriesForCartAsync(Cart cart);
}
