using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyShop.Entities;
using MyShop.Interfaces;
using MyShop.Services;
using Radzen;
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
    [Inject]
    public DialogService DialogService { get; set; }
    [Inject]
    public NotificationService NotificationService { get; set; }

    //current user name
    string userName;

    //checks if user is logged in
    private bool userIsLoggedIn;

    //used to store users products
    public List<Entities.CartProduct> CartProducts { get; set; } = new();

    //valuta
    public CultureInfo cultureInfo = new("fr-FR");

    //making sure no funky stuff happens when adding and removing items
    bool updatingData;

    //sum that will be saved to cart
    decimal cartSum=0;

    //alert
    Variant variant = Variant.Filled;
    AlertSize size = AlertSize.Medium;
    AlertStyle alertStyle = AlertStyle.Primary;

    //curent user cart
    public Entities.Cart userCart = new();

    //curent user
    public Entities.ShopAppWebUser user = new();

    protected override async Task OnInitializedAsync()
    {

        await base.OnInitializedAsync();
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userClaimsPrincipal = authenticationState.User;

        userIsLoggedIn = userClaimsPrincipal.Identity.IsAuthenticated;

        if (userIsLoggedIn)
        {
            userName = userClaimsPrincipal.Identity.Name;
            user = await UserService.GetUserByNameAsync(userName);

            userCart = await CartService.GetUserCurrentCart(user);
            CartProducts = await CartProductService.GetAllCartProductEntriesForCartAsync(userCart);

            foreach(CartProduct cp in CartProducts)
            {
                cp.Product = await ProductService.GetProductByIdAsync(cp.ProductsId);
            }

            foreach(var cartProduct in CartProducts)
            {
                cartSum += cartProduct.ProductQuantity * cartProduct.Product.Price.Value;
            }
            userCart.CartCost = cartSum;
            var SavedCartSum = await CartService.SaveCartSum(userCart);
        }

    }

    private void GoToProductDetails(int productId)
    {
        NavigationManager.NavigateTo($"products/{productId}");
    }


    private async void UpdateQuantity(CartProduct productToUpdate, int newValue)
    {
        cartSum += (newValue-productToUpdate.ProductQuantity) * productToUpdate.Product.Price.Value;
        userCart.CartCost = cartSum;
        var SavedCartSum = await CartService.SaveCartSum(userCart);
        productToUpdate.ProductQuantity=newValue;
        var quantityDecreaseSuccessfull = await CartProductService.UpdateQuantityForProductInCart(productToUpdate);
        if(quantityDecreaseSuccessfull.ProductQuantity <= 0)
        {
            NavigationManager.NavigateTo("/Cart", true);
        }
    }


    private async void Checkout()
    {

        var confirmCheckout = await DialogService.Confirm($"Are you sure you want to checkout with a total of ${cartSum}", "Confirm Checkout", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmCheckout.GetValueOrDefault())
        {
            var finishedCart = await CartService.LockCart(userCart);
            var newUserCart = await CartService.CreateNewCart(user.Id);
            userCart = newUserCart;
            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Order saved", Detail = "", Duration = 4000 });
            NavigationManager.NavigateTo("/Cart", true);
        }
    }

    void ShowNotification(NotificationMessage message)
    {
        NotificationService.Notify(message);
    }

}
