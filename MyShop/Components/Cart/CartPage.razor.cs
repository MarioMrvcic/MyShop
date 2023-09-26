using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Interfaces;
using MyShop.Services;
using System.Globalization;
using System.Security.Claims;

namespace MyShop.Components.Cart;

public partial class CartPage : ComponentBase
{
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    public ICartService CartService { get; set; }
    [Inject]
    public ICartProductService CartProductService { get; set; }
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public IUserService UserService { get; set; }

    //current user name
    string userName;

    //checks if user is logged in
    private bool userIsLoggedIn;

    //used to store users products
    public List<Entities.CartProduct> CartProducts { get; set; } = new();

    //valuta
    CultureInfo cultureInfo = new("fr-FR");

    //making sure no funky stuff happens when adding and removing items
    bool updatingData;

    protected override async Task OnInitializedAsync()
    {

        await base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userClaimsPrincipal = authenticationState.User;

        userIsLoggedIn = userClaimsPrincipal.Identity.IsAuthenticated;

        if (userIsLoggedIn)
        {
            userName = userClaimsPrincipal.Identity.Name;
            var user = await UserService.GetUserByNameAsync(userName);

            var userCart = await CartService.GetUserCurrentCart(user);
            CartProducts = await CartProductService.GetAllCartProductEntriesForCartAsync(userCart);

            foreach(CartProduct cp in CartProducts)
            {
                cp.Product = await ProductService.GetProductByIdAsync(cp.ProductsId);
            }
        }

    }

    private void GoToProductDetails(int productId)
    {
        NavigationManager.NavigateTo($"products/{productId}");
    }


    private async void IncreaseQuantity(CartProduct productToIncrease)
    {
        updatingData = true;
        var quantityIncreaseSuccessfull = await CartProductService.IncreaseQuantityForProductInCart(productToIncrease);
        updatingData = false;
        NavigationManager.NavigateTo("/Cart", true);
    }

    private async void DecreaseQuantity(CartProduct productToDecrease)
    {
        updatingData = true;
        var quantityDecreaseSuccessfull = await CartProductService.DecreaseQuantityForProductInCart(productToDecrease);
        updatingData = false;
        NavigationManager.NavigateTo("/Cart", true);
        if (productToDecrease.ProductQuantity <= 0)
        {
            NavigationManager.NavigateTo("/Cart", true);
        }
    }
}
